using EduFlow.BLL.Common.Validators.Payments.Interfaces;
using EduFlow.BLL.DTOs.Payments.Payment;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Payments.Services;

public class PaymentValidator(
    IValidator<PaymentForCreateDto> createValidator,
    IValidator<PaymentForUpdateDto> updateValidator) : IPaymentValidator
{
    private readonly IValidator<PaymentForCreateDto> _createValidator = createValidator;
    private readonly IValidator<PaymentForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(PaymentForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(PaymentForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
