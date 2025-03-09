using EduFlow.BLL.DTOs.Users.Teacher;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Users.Teacher;

public class TeacherForUpdateValidator : AbstractValidator<TeacherForUpdateDto>
{
    public TeacherForUpdateValidator()
    {
        RuleFor(x => x.Skills)
            .NotEmpty().WithMessage("Skill bo'lishi shart")
            .Must(x => x.All(s => !string.IsNullOrEmpty(s))).WithMessage("Bo'sh bo'lishi mumkun emas");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User Id si 0 dan katta bo'lishi kerak");
    }
}
