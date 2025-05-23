﻿using EduFlow.BLL.DTOs.Users.Teacher;

namespace EduFlow.BLL.Interfaces.Users;

public interface ITeacherService
{
    Task<IEnumerable<TeacherForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TeacherForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(TeacherForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, TeacherForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<TeacherForResultDto> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TeacherForResultDto>> GetByCourseIdAsync(long courseId, CancellationToken cancellationToken = default);
}
