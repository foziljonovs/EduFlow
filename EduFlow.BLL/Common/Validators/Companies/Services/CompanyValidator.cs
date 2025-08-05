using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.Company;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Companies.Services;

public class CompanyValidator(
    IValidator<CompanyForCreateDto> createValidator,
    IValidator<CompanyForUpdateDto> updateValidator) : ICompanyValidator
{
    private readonly IValidator<CompanyForCreateDto> _createValidator = createValidator;
    private readonly IValidator<CompanyForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(CompanyForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(CompanyForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
