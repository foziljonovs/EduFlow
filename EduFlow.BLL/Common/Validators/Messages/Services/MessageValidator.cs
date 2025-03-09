using EduFlow.BLL.Common.Validators.Messages.Interfaces;
using EduFlow.BLL.DTOs.Messages.Message;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Messages.Services;

public class MessageValidator(
    IValidator<MessageForCreateDto> createValidator) : IMessageValidator
{
    private readonly IValidator<MessageForCreateDto> _createValidator = createValidator;
    public async Task<ValidationResult> ValidateCreate(MessageForCreateDto dto)
        => await _createValidator.ValidateAsync(dto);
}
