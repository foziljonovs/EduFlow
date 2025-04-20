using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.StudentCourse;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Services;

public class StudentCourseValidator(
    IValidator<StudentCourseForCreateDto> createValidator,
    IValidator<StudentCourseForUpdateDto> updateValidator) : IStudentCourseValidator
{
    private readonly IValidator<StudentCourseForCreateDto> _createValidator = createValidator;
    private readonly IValidator<StudentCourseForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(StudentCourseForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(StudentCourseForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
