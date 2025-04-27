using EduFlow.BLL.DTOs.Courses.Lesson;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Interfaces;

public interface ILessonValidator
{
    Task<ValidationResult> ValidateCraete(LessonForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(LessonForUpdateDto dto);
}
