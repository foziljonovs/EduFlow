using EduFlow.BLL.DTOs.Users.User;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Users.User;

public class UserForChangePasswordValidator : AbstractValidator<UserForChangePasswordDto>
{
    public UserForChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yangi parol kiritilishi shart")
            .MinimumLength(4).WithMessage("Yangi parol 4 ta belgidan kam bo'lmasligi kerak")
            .MaximumLength(8).WithMessage("Yangi parol 8 ta belgidan uzun bo'lmasligi kerak")
            .Matches("[A-Za-z]").WithMessage("Yangi parol kamida 1 ta harf bo'lishi kerak")
            .Matches("[0-9]").WithMessage("Yangi parol kamida 1 ta son bo'lishi kerak");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Joriy parol kiritilishi shart")
            .MinimumLength(4).WithMessage("Joriy parol 4 ta belgidan kam bo'lmasligi kerak")
            .MaximumLength(8).WithMessage("Joriy parol 8 ta belgidan uzun bo'lmasligi kerak")
            .Matches("[A-Za-z]").WithMessage("Joriy parol kamida 1 ta harf bo'lishi kerak")
            .Matches("[0-9]").WithMessage("Joriy parol kamida 1 ta son bo'lishi kerak");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam kiritilishi shart")
            .Must(BedValidPhoneNumber).WithMessage("Telefon raqam noto'g'ri formatda");
    }

    private bool BedValidPhoneNumber(string phoneNumber)
        => !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 13 && phoneNumber.StartsWith("+998");
}
