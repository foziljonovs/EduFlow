using EduFlow.BLL.DTOs.Companies.Logo;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Companies.Logo;

public class LogoForCreateValidator : AbstractValidator<LogoForCreateDto>
{
    public LogoForCreateValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File nomi bo'sh bo'lmasigi kerak!");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Fayl turi bo'sh bo'lmasligi kerak!")
            .MinimumLength(2).WithMessage("Fayl turida eng kamida 2 ta belgi bo'lishi kerak!");

        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("Fayl joylashuvi bo'sh bo'lmasligi kerak!");
    }
}
