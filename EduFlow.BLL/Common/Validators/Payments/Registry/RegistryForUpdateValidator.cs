using EduFlow.BLL.DTOs.Payments.Registry;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Payments.Registry;

public class RegistryForUpdateValidator : AbstractValidator<RegistryForUpdateDto>
{
    public RegistryForUpdateValidator()
    {
        RuleFor(x => x.Debit)
            .GreaterThanOrEqualTo(0).WithMessage("Debit 0 dan kichik bo'lishi mumkun emas");

        RuleFor(x => x.Credit)
            .GreaterThanOrEqualTo(0).WithMessage("Credit 0 dan kichik bo'lishi mumkun emas");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Kirim yoki chiqimdan biri tanlanishi shart");
    }
}
