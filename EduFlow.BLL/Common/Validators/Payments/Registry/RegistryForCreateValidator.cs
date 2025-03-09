using EduFlow.BLL.DTOs.Payments.Registry;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Payments.Registry;

public class RegistryForCreateValidator : AbstractValidator<RegistryForCreateDto>
{
    public RegistryForCreateValidator()
    {
        RuleFor(x => x.Debit)
            .GreaterThanOrEqualTo(0).WithMessage("Debit 0 dan kichik bo'lishi mumkun emas");

        RuleFor(x => x.Credit)
            .GreaterThanOrEqualTo(0).WithMessage("Credit 0 dan kichik bo'lishi mumkun emas");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Kirim yoki chiqimdan biri tanlanishi shart");

        RuleFor(x => x)
            .Must(x => x.Debit != 0 || x.Credit != 0).WithMessage("Debit yoki Credit 2 lasidan bittasi 0 dan farqli bo'lishi kerak")
            .Must(x => x.Debit != 0 && x.Credit != 0).WithMessage("Debit va Credit bir vaqtda 0 dan farqli bo'lishi mumkun emas");
    }
}
