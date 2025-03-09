using EduFlow.BLL.Common.Validators.Users.Interface;
using EduFlow.BLL.Common.Validators.Users.User;
using EduFlow.BLL.DTOs.Users.User;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Users.Services;

public class UserValidator(
    IValidator<UserForCreateDto> createValidator,
    IValidator<UserForUpdateDto> updateValidator,
    IValidator<UserForChangePasswordDto> changePasswordValidator,
    IValidator<UserForLoginDto> loginValidator) : IUserValidator
{
    private readonly IValidator<UserForCreateDto> _createValidator = createValidator;
    private readonly IValidator<UserForUpdateDto> _updateValidator = updateValidator;
    private readonly IValidator<UserForChangePasswordDto> _changePasswordValidator = changePasswordValidator;
    private readonly IValidator<UserForLoginDto> _loginValidator = loginValidator;

    public async Task<ValidationResult> ValidateChangePassword(UserForChangePasswordDto dto)
        => await _changePasswordValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateCreate(UserForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateLogin(UserForLoginDto dto)
        => await _loginValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(UserForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
