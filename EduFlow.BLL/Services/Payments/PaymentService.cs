using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Payments.Interfaces;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.BLL.Interfaces.Payments;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Payments;

public class PaymentService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<PaymentService> logger,
    IPaymentValidator validator,
    IRegistryService registryService) : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PaymentService> _logger = logger;
    private readonly IPaymentValidator _validator = validator;
    private readonly IRegistryService _registryService = registryService;
    public async Task<bool> AddToPayAsync(PaymentForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsStudent = await _unitOfWork.Student.GetAsync(dto.StudentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            var existsGroup = await _unitOfWork.Group.GetAsync(dto.GroupId);
            if (existsGroup is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var existsRegistry = await _unitOfWork.Registry.GetAsync(dto.RegistryId);
            if (existsRegistry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            if (string.IsNullOrEmpty(dto.ReceiptNumber))
                dto.ReceiptNumber = Guid.NewGuid().ToString();

            var savedPayment = _mapper.Map<Payment>(dto);

            double paymentAmount = dto.Amount;
            if (dto.Discount > 0)
                paymentAmount = dto.Amount - ((dto.Amount / 100) * dto.Discount);

            var registry = new RegistryForCreateDto
            {
                Debit = paymentAmount,
                Credit = 0,
                Description = dto.Discount > 0
                    ? $"Discounted payment ({dto.Discount}%) for {existsStudent.Fullname} in {existsGroup.Name} group."
                    : $"Payment for {existsStudent.Fullname} in {existsGroup.Name} group.",
                Type = dto.Type,
                IsConfirmed = true
            };

            var savedRegistry = await _registryService.IncomeAsync(registry, cancellationToken);

            if (!savedRegistry)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "An error occurred while adding the registry for payment.");

            savedPayment.Status = PaymentStatus.Pending;

            return await _unitOfWork.Payment.AddConfirmAsync(savedPayment);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while adding the payment. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var payment = await _unitOfWork.Payment.GetAsync(id);
            if (payment is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payment not found.");

            var registry = await _unitOfWork.Registry.GetAsync(payment.RegistryId);

            if (registry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            if (registry.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This registry has been deleted.");

            var paymentAmount = payment.Amount;
            if (payment.Discount > 0)
                paymentAmount = payment.Amount - ((payment.Amount / 100) * payment.Discount);

            var deletedRegistry = new RegistryForCreateDto
            {
                Debit = 0,
                Credit = paymentAmount,
                Description = $"Payment deleted for {payment.Student.Fullname} in {payment.Group.Name} group.",
                Type = payment.Type,
                IsConfirmed = true
            };

            var savedRegistry = await _registryService.OutlayAsync(deletedRegistry, cancellationToken);

            if (!savedRegistry)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "An error occurred while adding the registry for payment deletion.");

            payment.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the payment id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentForResultDto>> GetAllAsync(CancellationToken cancellation = default)
    {
        try
        {
            var payments = await _unitOfWork.Payment
                .GetAllAsync()
                .Where(x => x.IsDeleted == false)
                .ToListAsync(cancellation);

            if (!payments.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            return _mapper.Map<IEnumerable<PaymentForResultDto>>(payments);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all payment. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentForResultDto>> GetAllByGroupIdAsync(long groupId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await _unitOfWork.Payment
                .GetAllAsync()
                .Where(x => x.GroupId == groupId && x.IsDeleted == false)
                .ToListAsync(cancellationToken);

            if (!payments.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            return _mapper.Map<IEnumerable<PaymentForResultDto>>(payments);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all payment by course id: {groupId}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await _unitOfWork.Payment
                .GetAllAsync()
                .Where(x => x.StudentId == studentId && x.IsDeleted == false)
                .ToListAsync(cancellationToken);

            if (!payments.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            return _mapper.Map<IEnumerable<PaymentForResultDto>>(payments);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all payment by student id: {studentId}. {ex}");
            throw;
        }
    }

    public async Task<PaymentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var payment = await _unitOfWork.Payment.GetAsync(id);
            if (payment is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payment not found.");

            if (payment.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This payment has been deleted.");

            return _mapper.Map<PaymentForResultDto>(payment);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get payment by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateUpdate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsPayment = await _unitOfWork.Payment.GetAllAsync()
                .Where(x => x.Id == id && x.ReceiptNumber == dto.ReceiptNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (existsPayment is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payment not found.");

            if (existsPayment.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This payment has been deleted.");

            var existsStudent = await _unitOfWork.Student.GetAsync(dto.StudentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            var existsGroup = await _unitOfWork.Group.GetAsync(dto.GroupId);
            if (existsGroup is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var existsRegistry = await _unitOfWork.Registry.GetAsync(dto.RegistryId);
            if (existsRegistry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            double paymentAmount = dto.Amount;
            if(dto.Discount > 0)
                paymentAmount = dto.Amount - ((dto.Amount / 100) * dto.Discount);

            var registry = new RegistryForUpdateDto
            {
                Debit = paymentAmount,
                Credit = 0,
                Type = dto.Type,
                Description = $"Payment for {existsStudent.Fullname} in {existsGroup.Name}",
                IsConfirmed = true
            };

            //var savedRegistry = await _registryService.UpdateAsync(registry, cancellationToken);
            //if (!savedRegistry)
            //    throw new StatusCodeException(HttpStatusCode.BadRequest, "Failed to create registry entry");

            var payment = _mapper.Map<Payment>(dto);
            payment.RegistryId = dto.RegistryId;
            payment.Status = PaymentStatus.Completed;
            payment.PaymentDate = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Payment.UpdateAsync(existsPayment);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update payment id: {id}. {ex}");
            throw;
        }
    }
}
