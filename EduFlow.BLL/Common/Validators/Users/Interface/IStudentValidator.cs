using EduFlow.BLL.DTOs.Users.Student;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Users.Interface;

public interface IStudentValidator
{
    Task<ValidationResult> ValidateCreate(StudentForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(StudentForUpdateDto dto);
}
