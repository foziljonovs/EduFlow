using EduFlow.BLL.DTOs.Payments.Payment;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Payments.Interfaces;

public interface IPaymentValidator
{
    Task<ValidationResult> ValidateCreate(PaymentForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(PaymentForUpdateDto dto);
}
