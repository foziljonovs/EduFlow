using EduFlow.BLL.Common.Validators.Payments.Interfaces;
using EduFlow.BLL.DTOs.Payments.Registry;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Payments.Services;

public class RegistryValidator(
    IValidator<RegistryForCreateDto> createValidator,
    IValidator<RegistryForUpdateDto> updateValidator) : IRegistryValidator
{
    private readonly IValidator<RegistryForCreateDto> _createValidator = createValidator;
    private readonly IValidator<RegistryForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(RegistryForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(RegistryForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
