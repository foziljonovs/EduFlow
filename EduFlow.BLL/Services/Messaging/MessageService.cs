using EduFlow.BLL.DTOs.Messages.Message;
using EduFlow.BLL.Interfaces.Messaging;

namespace EduFlow.BLL.Services.Messaging;

public class MessageService : IMessageService
{
    public Task<IEnumerable<MessageForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MessageForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MessageForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<MessageForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendAsync(MessageForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
