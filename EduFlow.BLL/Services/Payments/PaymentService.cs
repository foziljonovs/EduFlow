using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Pagination;
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
    public async Task<long> AddToPayAsync(PaymentForCreateDto dto, CancellationToken cancellationToken = default)
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

            var existsTeacher = await _unitOfWork.Teacher.GetAsync(dto.TeacherId);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var existsRegistry = await _unitOfWork.Registry.GetAsync(dto.RegistryId);
            if (existsRegistry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            if (string.IsNullOrEmpty(dto.ReceiptNumber))
                dto.ReceiptNumber = Guid.NewGuid().ToString();

            var savedPayment = _mapper.Map<Payment>(dto);
            savedPayment.Status = PaymentStatus.Pending;

            return await _unitOfWork.Payment.AddAsync(savedPayment);
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

            var group = await _unitOfWork.Group.GetAsync(payment.GroupId);
            if (group is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var teacher = await _unitOfWork.Teacher.GetAsync(payment.TeacherId);
            if (teacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var registry = await _unitOfWork.Registry.GetAsync(payment.RegistryId);
            if (registry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            if (registry.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This registry has been deleted.");

            payment.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the payment id: {id}. {ex}");
            throw;
        }
    }

    public async Task<PagedList<PaymentForResultDto>> FilterAsync(int pageSize, int pageNumber, PaymentForFilterDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var paymentQuery = _unitOfWork.Payment
                .GetAllFullInformation();

            if (!paymentQuery.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            if(dto.StartedDate.HasValue && dto.FinishedDate.HasValue)
            {
                var startedDateUtc = DateTime.SpecifyKind(dto.StartedDate.Value, DateTimeKind.Utc);
                var finishedDateUtc = DateTime.SpecifyKind(dto.FinishedDate.Value, DateTimeKind.Utc);
                paymentQuery = paymentQuery.Where(x =>
                    x.PaymentDate >= startedDateUtc &&
                    x.PaymentDate <= finishedDateUtc);
            }

            if(dto.CourseId > 0)
                paymentQuery = paymentQuery
                    .Where(x => x.Group.CourseId == dto.CourseId);

            if (dto.TeacherId > 0)
                paymentQuery = paymentQuery
                    .Where(x => x.TeacherId == dto.TeacherId);

            if(dto.Status.HasValue)
                paymentQuery = paymentQuery
                    .Where(x => x.Status == dto.Status.Value);

            if(dto.Type.HasValue)
                paymentQuery = paymentQuery
                    .Where(x => x.Type == dto.Type.Value);

            var paymentCount = await paymentQuery.CountAsync(cancellation);

            if (paymentCount == 0)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            var mappedPayments = await paymentQuery
                .Select(p => _mapper.Map<PaymentForResultDto>(p))
                .ToListAsync(cancellation);

            var pagedlist = new PagedList<PaymentForResultDto>(
                mappedPayments,
                mappedPayments.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedPayments, pageSize, pageNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while filter the payment. {ex}");
            throw;
        }
    }

    public async Task<PagedList<PaymentForResultDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken cancellation = default)
    {
        try
        {
            var payments = await _unitOfWork.Payment
                .GetAllFullInformation()
                .ToListAsync(cancellation);

            if (!payments.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            var mappedPayments = payments
                .Select(p => _mapper.Map<PaymentForResultDto>(p))
                .ToList();

            var pagedlist = new PagedList<PaymentForResultDto>(
                mappedPayments,
                mappedPayments.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedPayments, pageSize, pageNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all payment. {ex}");
            throw;
        }
    }

    public async Task<PagedList<PaymentForResultDto>> GetAllByGroupIdAsync(int pageSize, int pageNumber, long groupId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await _unitOfWork.Payment
                .GetAllFullInformation()
                .Where(x => x.GroupId == groupId)
                .ToListAsync(cancellationToken);

            if (!payments.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            var mappedPayments = payments
                .Select(p => _mapper.Map<PaymentForResultDto>(p))
                .ToList();

            var pagedlist = new PagedList<PaymentForResultDto>(
                mappedPayments,
                mappedPayments.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedPayments, pageSize, pageNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all payment by course id: {groupId}. {ex}");
            throw;
        }
    }

    public async Task<PagedList<PaymentForResultDto>> GetAllByStudentIdAsync(int pageSize, int pageNumber, long studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await _unitOfWork.Payment
                .GetAllFullInformation()
                .Where(x => x.StudentId == studentId)
                .ToListAsync(cancellationToken);

            if (!payments.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            var mappedPayments = payments
                .Select(p => _mapper.Map<PaymentForResultDto>(p))
                .ToList();

            var pagedlist = new PagedList<PaymentForResultDto>(
                mappedPayments,
                mappedPayments.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedPayments, pageSize, pageNumber);
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
            var payment = await _unitOfWork.Payment.GetAllFullInformation()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (payment is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payment not found.");

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

            var existsTeacher = await _unitOfWork.Teacher.GetAsync(dto.TeacherId);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var existsGroup = await _unitOfWork.Group.GetAsync(dto.GroupId);
            if (existsGroup is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var existsRegistry = await _unitOfWork.Registry.GetAsync(dto.RegistryId);
            if (existsRegistry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            _mapper.Map(dto, existsPayment);
            existsPayment.Id = id;
            existsPayment.StudentId = existsStudent.Id;
            existsPayment.TeacherId = existsTeacher.Id;
            existsPayment.GroupId = existsGroup.Id;
            existsPayment.RegistryId = existsRegistry.Id;
            existsPayment.PaymentDate = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Payment.UpdateAsync(existsPayment);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update payment id: {id}. {ex}");
            throw;
        }
    }
}
