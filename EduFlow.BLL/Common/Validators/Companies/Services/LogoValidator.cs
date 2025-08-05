using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.Logo;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Companies.Services;

public class LogoValidator(
    IValidator<LogoForCreateDto> createValidator,
    IValidator<LogoForUpdateDto> updateValidator) : ILogoValidator
{
    private readonly IValidator<LogoForCreateDto> _createValidator = createValidator;
    private readonly IValidator<LogoForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(LogoForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(LogoForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
