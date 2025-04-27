using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.Lesson;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Services;

public class LessonValidator(
    IValidator<LessonForCreateDto> craeteValidator,
    IValidator<LessonForUpdateDto> updateValidator) : ILessonValidator
{
    private readonly IValidator<LessonForCreateDto> _createValidator = craeteValidator;
    private readonly IValidator<LessonForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCraete(LessonForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(LessonForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
