using EduFlow.BLL.DTOs.Users.Teacher;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Users.Interface;

public interface ITeacherValidator
{
    Task<ValidationResult> ValidateCreate(TeacherForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(TeacherForUpdateDto dto);
}
