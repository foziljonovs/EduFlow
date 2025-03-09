using Microsoft.AspNetCore.SignalR;

namespace EduFlow.BLL.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationAsync(string teacherId, string message)
    {
        await Clients
            .User(teacherId)
            .SendAsync("ReceiveMessage", message);
    }
}
