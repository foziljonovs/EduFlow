using EduFlow.BLL.DTOs.Companies.Company;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Companies.Interfaces;

public interface ICompanyValidator
{
    Task<ValidationResult> ValidateCreate(CompanyForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(CompanyForUpdateDto dto);
}
