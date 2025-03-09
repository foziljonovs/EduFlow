using EduFlow.BLL.DTOs.Courses.Course;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Interface;

public interface ICourseValidator
{
    Task<ValidationResult> ValidateCreate(CourseForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(CourseForUpdateDto dto);
}
