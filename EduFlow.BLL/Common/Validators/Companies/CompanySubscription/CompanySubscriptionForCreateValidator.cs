using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Companies.CompanySubscription;

public class CompanySubscriptionForCreateValidator : AbstractValidator<CompanySubscriptionForCreateDto>
{
    public CompanySubscriptionForCreateValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0).WithMessage("Company Id si 0 dan katta bo'lishi kerak!");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Obuna turi tanlanishi kerak!");
    }
}
