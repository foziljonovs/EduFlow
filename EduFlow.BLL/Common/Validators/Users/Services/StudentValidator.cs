using EduFlow.BLL.Common.Validators.Users.Interface;
using EduFlow.BLL.DTOs.Users.Student;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Users.Services;

public class StudentValidator(
    IValidator<StudentForCreateDto> createValidator,
    IValidator<StudentForUpdateDto> updateValidator) : IStudentValidator
{
    private readonly IValidator<StudentForCreateDto> _createValidator = createValidator;
    private readonly IValidator<StudentForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(StudentForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(StudentForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
