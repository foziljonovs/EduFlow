using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.Group;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Services;

public class GroupValidator(
    IValidator<GroupForCreateDto> createValidator,
    IValidator<GroupForUpdateDto> updateValidator) : IGroupValidator
{
    private readonly IValidator<GroupForCreateDto> _createValidator = createValidator;
    private readonly IValidator<GroupForUpdateDto> _updateValidator = updateValidator;
    public async Task<ValidationResult> ValidateCreate(GroupForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(GroupForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
