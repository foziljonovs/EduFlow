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
