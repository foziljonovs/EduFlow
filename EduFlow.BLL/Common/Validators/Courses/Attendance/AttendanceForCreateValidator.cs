﻿using EduFlow.BLL.DTOs.Courses.Attendance;
using FluentValidation;

namespace EduFlow.BLL.Common.Validators.Courses.Attendance;

public class AttendanceForCreateValidator : AbstractValidator<AttendanceForCraeteDto>
{
    public AttendanceForCreateValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("O'quvchi Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.LessonId)
            .GreaterThan(0).WithMessage("Dars Id si 0 dan katta bo'lishi kerak");

        RuleFor(x => x.Date)
            .NotNull().WithMessage("Sana bo'lishi shart");
    }
}
