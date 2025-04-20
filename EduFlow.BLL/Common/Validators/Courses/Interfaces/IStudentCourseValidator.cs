using EduFlow.BLL.DTOs.Courses.StudentCourse;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Interfaces;

public interface IStudentCourseValidator
{
    Task<ValidationResult> ValidateCreate(StudentCourseForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(StudentCourseForUpdateDto dto);
}
