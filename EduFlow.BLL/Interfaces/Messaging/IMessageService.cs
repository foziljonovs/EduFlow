using EduFlow.BLL.DTOs.Messages.Message;

namespace EduFlow.BLL.Interfaces.Messaging;

public interface IMessageService
{
    Task<IEnumerable<MessageForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MessageForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> SendAsync(MessageForCreateDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<MessageForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MessageForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
}
