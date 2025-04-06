using EduFlow.BLL.DTOs.Courses.Group;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Interfaces;

public interface IGroupValidator
{
    Task<ValidationResult> ValidateCreate(GroupForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(GroupForUpdateDto dto);
}
