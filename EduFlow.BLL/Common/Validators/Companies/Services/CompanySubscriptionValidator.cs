using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Companies.Services;

public class CompanySubscriptionValidator(
    IValidator<CompanySubscriptionForCreateDto> createValidator,
    IValidator<CompanySubscriptionForUpdateDto> updateValidator) : ICompanySubscriptionValidator
{ 
    private readonly IValidator<CompanySubscriptionForCreateDto> _createValidator = createValidator;
    private readonly IValidator<CompanySubscriptionForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(CompanySubscriptionForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(CompanySubscriptionForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
