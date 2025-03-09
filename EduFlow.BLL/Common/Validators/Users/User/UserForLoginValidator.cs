using EduFlow.BLL.DTOs.Users.User;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Users.User;

public class UserForLoginValidator : AbstractValidator<UserForLoginDto>
{
    public UserForLoginValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Parol kiritilishi shart")
            .MinimumLength(4).WithMessage("Parol 4 ta belgidan kam bo'lmasligi kerak")
            .MaximumLength(8).WithMessage("Parol 8 ta belgidan uzun bo'lmasligi kerak")
            .Matches("[A-Za-z]").WithMessage("Parol kamida 1 ta harf bo'lishi kerak")
            .Matches("[0-9]").WithMessage("Parol kamida 1 ta son bo'lishi kerak");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam kiritilishi shart")
            .Must(BedValidPhoneNumber).WithMessage("Telefon raqam noto'g'ri formatda");
    }

    private bool BedValidPhoneNumber(string phoneNumber)
        => !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 13 && phoneNumber.StartsWith("+998");
}
