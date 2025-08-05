using EduFlow.BLL.DTOs.Companies.Company;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Companies.Company;

public class CompanyForUpdateValidator : AbstractValidator<CompanyForUpdateDto>
{
    public CompanyForUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nom kiritilishi mavburiy!")
            .MinimumLength(3).WithMessage("O'quv markaz nomi eng kamida 3 ta belgi bo'lishi kerak!");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam kiritilishi shart!")
            .Must(BedValidPhoneNumber).WithMessage("Telefon raqam 13 ta belgidan va +998 bilan boshlangan bo'lishi kerak");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Manzil kiritilishi shart!");
    }

    private bool BedValidPhoneNumber(string phoneNumber)
        => !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 13 && phoneNumber.StartsWith("+998");
}
