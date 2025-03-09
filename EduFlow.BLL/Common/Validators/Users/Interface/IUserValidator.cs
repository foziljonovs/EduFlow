using EduFlow.BLL.Common.Validators.Users.User;
using EduFlow.BLL.DTOs.Users.User;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Users.Interface;

public interface IUserValidator
{
    Task<ValidationResult> ValidateCreate(UserForCreateDto dto);
    Task<ValidationResult> ValidateUpdate(UserForUpdateDto dto);
    Task<ValidationResult> ValidateChangePassword(UserForChangePasswordDto dto);
    Task<ValidationResult> ValidateLogin(UserForLoginDto dto);
}
