using EduFlow.BLL.Common.Validators.Courses.Interface;
using EduFlow.BLL.DTOs.Courses.Course;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Services;

public class CourseValidator(
    IValidator<CourseForCreateDto> createValidator,
    IValidator<CourseForUpdateDto> updateValidator) : ICourseValidator
{
    private readonly IValidator<CourseForCreateDto> _createValidator = createValidator;
    private readonly IValidator<CourseForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(CourseForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(CourseForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
