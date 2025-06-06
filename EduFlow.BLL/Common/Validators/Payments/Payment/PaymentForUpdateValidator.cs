using EduFlow.BLL.DTOs.Payments.Payment;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Payments.Payment;

public class PaymentForUpdateValidator : AbstractValidator<PaymentForUpdateDto>
{
    public PaymentForUpdateValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("O'quvchi IDsi 0 dan katta bo'lishi kerak");

        RuleFor(x => x.GroupId)
            .GreaterThan(0).WithMessage("Guruh IDsi 0 dan katta bo'lishi kerak");

        RuleFor(x => x.RegistryId)
            .GreaterThan(0).WithMessage("Registr IDsi 0 dan katta bo'lishi kerak");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Chegirma 0 dan kam bo'lishi mumkin emas")
            .LessThanOrEqualTo(100).When(x => x.Discount > 0)
            .WithMessage("Chegirma 100% dan ko'p bo'lishi mumkin emas");

        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("To'lov sanasi kiritilishi shart")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("To'lov sanasi kelajakda bo'lishi mumkin emas");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Noto'g'ri to'lov holati");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Noto'g'ri to'lov turi");

        RuleFor(x => x.ReceiptNumber)
            .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.ReceiptNumber))
            .WithMessage("Qabul raqami 50 ta belgidan oshmasligi kerak");

        RuleFor(x => x.Notes)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Izoh 500 ta belgidan oshmasligi kerak");
    }
}
