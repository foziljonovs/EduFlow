using EduFlow.BLL.DTOs.Courses.Group;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Courses.Group;

public class GroupForUpdateValidator : AbstractValidator<GroupForUpdateDto>
{
    public GroupForUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Guruh nomi kiritish majburiy")
            .MinimumLength(3).WithMessage("Guruh nomi minimum 3 ta belgidan iborat bo'lishi kerak")
            .MaximumLength(60).WithMessage("Guruh nomi maximum 60 ta belgidan iborat bo'lishi kerak");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Kurs Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.IsStatus)
            .IsInEnum().WithMessage("Guruh holati tanlanmagan");
    }
}
