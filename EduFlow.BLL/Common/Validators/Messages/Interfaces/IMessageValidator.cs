using EduFlow.BLL.DTOs.Messages.Message;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Messages.Interfaces;

public interface IMessageValidator
{
    Task<ValidationResult> ValidateCreate(MessageForCreateDto dto);
}
