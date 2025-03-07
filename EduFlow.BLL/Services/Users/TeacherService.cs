using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.BLL.Interfaces.Users;

namespace EduFlow.BLL.Services.Users;

public class TeacherService : ITeacherService
{
    public Task<bool> AddAsync(TeacherForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TeacherForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TeacherForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(TeacherForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
