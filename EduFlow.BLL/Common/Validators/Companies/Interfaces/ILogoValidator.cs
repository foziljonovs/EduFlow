using EduFlow.BLL.DTOs.Companies.Logo;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Companies.Interfaces;

public interface ILogoValidator
{
    Task<ValidationResult> ValidateCreate(LogoForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(LogoForUpdateDto dto);
}
