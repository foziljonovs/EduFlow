using EduFlow.BLL.DTOs.Courses.Course;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Courses.Course;

public class CourseForCreateValidator : AbstractValidator<CourseForCreateDto>
{
    public CourseForCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kurs nomi kiritish majburiy")
            .MinimumLength(3).WithMessage("Kurs nomi minimum 3 ta belgidan iborat bo'lishi kerak");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Kurs narxi 0 bo'lmasligi kerak");

        RuleFor(x => x.Archived)
            .IsInEnum().WithMessage("Kurs holati tanlanmagan");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0).WithMessage("O'qituvchi tayinlanishi kerak");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Kategoriya bo'lishi shart");
    }
}
