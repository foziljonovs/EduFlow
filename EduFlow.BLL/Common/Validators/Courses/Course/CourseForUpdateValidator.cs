using EduFlow.BLL.DTOs.Courses.Course;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Courses.Course;

public class CourseForUpdateValidator : AbstractValidator<CourseForUpdateDto>
{
    public CourseForUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kurs nomi kiritish majburiy")
            .MinimumLength(3).WithMessage("Kurs nomi minimum 3 ta belgidan iborat bo'lishi kerak");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Kurs narxi 0 bo'lmasligi kerak");

        RuleFor(x => x.Archived)
            .IsInEnum().WithMessage("Kurs holati tanlanmagan");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Kategoriya Id si 0 dan katta bo'lishi kerak");
    }
}
