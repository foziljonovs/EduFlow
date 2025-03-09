using EduFlow.BLL.DTOs.Courses.Attendance;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Interfaces;

public interface IAttendanceValidator
{
    Task<ValidationResult> ValidateCreate(AttendanceForCraeteDto dto);
    Task<ValidationResult> ValidateUpdate(AttendanceForUpdateDto dto);
}
