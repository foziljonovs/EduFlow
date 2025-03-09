using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.Attendance;
using FluentValidation;
using FluentValidation.Results;

namespace EduFlow.BLL.Common.Validators.Courses.Services;

public class AttendanceValidator(
    IValidator<AttendanceForCraeteDto> createValidator,
    IValidator<AttendanceForUpdateDto> updateaValidator) : IAttendanceValidator
{
    private readonly IValidator<AttendanceForCraeteDto> _craeteValidator = createValidator;
    private readonly IValidator<AttendanceForUpdateDto> _updateValidator = updateaValidator;
    public async Task<ValidationResult> ValidateCreate(AttendanceForCraeteDto dto)
        => await _craeteValidator.ValidateAsync(dto);

    public async Task<ValidationResult> ValidateUpdate(AttendanceForUpdateDto dto)
        => await _updateValidator.ValidateAsync(dto);
}
