using AutoMapper;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace EduFlow.BLL.Services.Courses;

public class LessonService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<LessonService> logger) : ILessonService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<LessonService> _logger = logger;
    public Task<bool> AddAsync(LessonForCreateDto lesson, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LessonForResultDto>> GetAllAsync(CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<LessonForResultDto> GetByIdAsync(long id, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(LessonForUpdateDto lesson, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}
