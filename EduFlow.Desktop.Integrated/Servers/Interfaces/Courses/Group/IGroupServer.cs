﻿using EduFlow.BLL.DTOs.Courses.Group;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Group;

public interface IGroupServer
{
    Task<List<GroupForResultDto>> GetAllAsync();
    Task<GroupForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(GroupForCreateDto dto);
    Task<bool> UpdateAsync(long id, GroupForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<GroupForResultDto>> GetAllByTeacherIdAsync(long teacherId);
    Task<List<GroupForResultDto>> GetAllByCourseIdAsync(long courseId);
    Task<List<GroupForResultDto>> FilterAsync(GroupForFilterDto dto);
    Task<bool> AddStudentsToGroupAsync(long groupId, List<long> students);
    Task<bool> DeleteForStudentAsync(long id, long studentId);
}
