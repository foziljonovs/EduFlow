using EduFlow.BLL.DTOs.Messages.Message;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Messages.Message;

public class MessageForCreateValidator : AbstractValidator<MessageForCreateDto>
{
    public MessageForCreateValidator()
    {
        RuleFor(x => x.GroupId)
            .GreaterThan(0).WithMessage("Kurs Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Xabar bo'sh bo'lmasligi kerak");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam bo'sh bo'lishi mumkun emas")
            .Must(DebValidPhoneNumber).WithMessage("Telefon raqam noto'g'ri formatda");
    }

    private bool DebValidPhoneNumber(string phoneNumber)
        => !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 13 && phoneNumber.StartsWith("+998");
}
