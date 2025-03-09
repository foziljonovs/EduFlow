using EduFlow.BLL.DTOs.Payments.Registry;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Payments.Interfaces;

public interface IRegistryValidator
{
    Task<ValidationResult> ValidateCreate(RegistryForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(RegistryForUpdateDto dto);
}
