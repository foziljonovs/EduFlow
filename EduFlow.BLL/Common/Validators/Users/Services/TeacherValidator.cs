using EduFlow.BLL.Common.Validators.Users.Interface;
using EduFlow.BLL.DTOs.Users.Teacher;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Users.Services;

public class TeacherValidator(
    IValidator<TeacherForCreateDto> createValidator,
    IValidator<TeacherForUpdateDto> updateValidator) : ITeacherValidator
{
    private readonly IValidator<TeacherForCreateDto> _createValidator = createValidator;
    private readonly IValidator<TeacherForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(TeacherForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(TeacherForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
