using EduFlow.BLL.DTOs.Courses.Lesson;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Courses.Lesson;

public class LessonForCraeteValidator : AbstractValidator<LessonForCreateDto>
{
    public LessonForCraeteValidator()
    {
        RuleFor(x => x.LessonNumber)
            .NotEmpty().WithMessage("Dars raqami bo'sh bo'lmasligi kerak")
            .GreaterThan(0).WithMessage("Dars raqami 0 dan katta bo'lishi kerak");

        RuleFor(x => x.GroupId)
            .NotEmpty().WithMessage("Guruh ID bo'sh bo'lmasligi kerak")
            .GreaterThan(0).WithMessage("Guruh ID 0 dan katta bo'lishi kerak");
    }
}
