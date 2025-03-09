using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Payments.Interfaces;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.Interfaces.Payments;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Payments;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Payments;

public class PaymentService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<PaymentService> logger,
    IPaymentValidator validator) : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PaymentService> _logger = logger;
    private readonly IPaymentValidator _validator = validator;
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

            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            var existsRegistry = await _unitOfWork.Registry.GetAsync(dto.RegistryId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            var savedPayment = _mapper.Map<Payment>(dto);

            return await _unitOfWork.Payment.AddConfirmAsync(savedPayment);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while added the payment. {ex}");
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

            payment.IsDeleted = true;
            return await _unitOfWork.Payment.UpdateAsync(payment);
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

    public async Task<IEnumerable<PaymentForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await _unitOfWork.Payment
                .GetAllAsync()
                .Where(x => x.CourseId == courseId && x.IsDeleted == false)
                .ToListAsync(cancellationToken);

            if (!payments.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payments not found.");

            return _mapper.Map<IEnumerable<PaymentForResultDto>>(payments);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all payment by course id: {courseId}. {ex}");
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

            var existsPayment = await _unitOfWork.Payment.GetAsync(id);
            if (existsPayment is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Payment not found.");

            if (existsPayment.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This payment has been deleted.");

            var existsStudent = await _unitOfWork.Student.GetAsync(dto.StudentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            var existsRegistry = await _unitOfWork.Registry.GetAsync(dto.RegistryId);
            if (existsRegistry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            var updatePayment = _mapper.Map<Payment>(dto);
            updatePayment.Id = id;
            updatePayment.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Payment.UpdateAsync(updatePayment);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update payment id: {id}. {ex}");
            throw;
        }
    }
}
