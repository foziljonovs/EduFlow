using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Messages;
using EduFlow.Domain.Entities.Messaging;

namespace EduFlow.DAL.Repositories.Messages;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(AppDbContext context) : base(context)
    {
        
    }
}
