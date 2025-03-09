using EduFlow.BLL.DTOs.Courses.Attendance;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Courses.Attendance;

public class AttendanceForCreateValidator : AbstractValidator<AttendanceForCraeteDto>
{
    public AttendanceForCreateValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("O'quvchi Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Kurs Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.Date)
            .NotNull().WithMessage("Sana bo'lishi shart");

        RuleFor(x => x.IsActived)
            .NotEmpty().WithMessage("Aktivlik tanlanishi kerak");
    }
}
