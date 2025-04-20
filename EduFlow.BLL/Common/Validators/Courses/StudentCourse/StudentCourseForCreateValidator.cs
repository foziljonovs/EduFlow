using EduFlow.BLL.DTOs.Courses.StudentCourse;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Courses.StudentCourse;

public class StudentCourseForCreateValidator : AbstractValidator<StudentCourseForCreateDto>
{
    public StudentCourseForCreateValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Talaba Id si kiritish majburiy")
            .GreaterThan(0).WithMessage("Talaba Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Kurs Id si kiritish majburiy")
            .GreaterThan(0).WithMessage("Kurs Id si 0 dan katta bo'lishi kerak");
    }
}
