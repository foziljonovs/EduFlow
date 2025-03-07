using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.Interfaces.Courses;

namespace EduFlow.BLL.Services.Courses;

public class CourseService : ICourseService
{
    public Task<bool> AddAsync(CourseForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CourseForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CourseForResultDto>> GetAllByCategoryIdAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CourseForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CourseForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, CourseForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
