using EduFlow.BLL.DTOs.Users.User;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Users.User;

public class UserForUpdateValidator : AbstractValidator<UserForUpdateDto>
{
    public UserForUpdateValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Ism bo'sh bo'lmasligi kerak");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Familya bo'sh bo'lmasligi kerak");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Parol kiritilishi shart")
            .MinimumLength(4).WithMessage("Parol 4 ta belgidan kam bo'lmasligi kerak")
            .MaximumLength(8).WithMessage("Parol 8 ta belgidan uzun bo'lmasligi kerak")
            .Matches("[A-Za-z]").WithMessage("Parol kamida 1 ta harf bo'lishi kerak")
            .Matches("[0-9]").WithMessage("Parol kamida 1 ta son bo'lishi kerak");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam kiritilishi shart")
            .Must(BedValidPhoneNumber).WithMessage("Telefon raqam noto'g'ri formatda");

        RuleFor(x => x.Age)
            .GreaterThan(14).WithMessage("User 14 yoshdan katta bo'lishi kerak");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Bunday ro'l mavjud emas");
    }

    private bool BedValidPhoneNumber(string phoneNumber)
        => !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 13 && phoneNumber.StartsWith("+998");
}
