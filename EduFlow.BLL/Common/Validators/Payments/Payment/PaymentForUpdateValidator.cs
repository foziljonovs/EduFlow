using EduFlow.BLL.DTOs.Payments.Payment;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Payments.Payment;

public class PaymentForUpdateValidator : AbstractValidator<PaymentForUpdateDto>
{
    public PaymentForUpdateValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("O'quvchi Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Kurs Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.RegistryId)
            .GreaterThan(0).WithMessage("Registry Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("To'lov summasi 0 dan baland bo'lishi shart");
    }
}
