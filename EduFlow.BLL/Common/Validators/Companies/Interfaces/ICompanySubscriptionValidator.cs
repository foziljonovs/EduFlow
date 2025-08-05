using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Companies.Interfaces;

public interface ICompanySubscriptionValidator
{
    Task<ValidationResult> ValidateCreate(CompanySubscriptionForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(CompanySubscriptionForUpdateDto dto);
}
