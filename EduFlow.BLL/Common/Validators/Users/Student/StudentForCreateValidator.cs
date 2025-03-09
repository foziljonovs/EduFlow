using EduFlow.BLL.DTOs.Users.Student;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Users.Student;

public class StudentForCreateValidator : AbstractValidator<StudentForCreateDto>
{
    public StudentForCreateValidator()
    {
        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("Ism bo'sh bo'lishi mumkun emas")
            .MinimumLength(3).WithMessage("Eng kamida 3 ta belgidan iborat bo'lishi kerak");

        RuleFor(x => x.Age)
            .GreaterThan(7).WithMessage("7 yoshdan yuqori bo'lishi kerak");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam bo'sh bo'lmasligi kerak")
            .Must(BedValidPhoneNumber).WithMessage("Telefon raqam noto'g'ri formatda");
    }

    private bool BedValidPhoneNumber(string phoneNumber)
        => !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 13 && phoneNumber.StartsWith("+998");
}
