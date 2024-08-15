using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Notifications.Api
{
    public class NotificationsHub : Hub<INotificationClient>
    {
        public Task SendNotification(string message)
        {
            return Clients.All.ReceiveNotification(message);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification($"Thank you for connecting {Context.User?.Identity?.Name}");
            await base.OnConnectedAsync();
        }
    }

    public interface INotificationClient
    {
        Task ReceiveNotification(string message);
    }
}
