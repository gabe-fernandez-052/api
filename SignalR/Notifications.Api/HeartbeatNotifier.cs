using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notifications.Api
{
    public class HeartbeatNotifier : BackgroundService
    {
        private readonly IHubContext<NotificationsHub, INotificationClient> _context;

        public HeartbeatNotifier(IHubContext<NotificationsHub, INotificationClient> context)
        {
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _context.Clients.All.ReceiveNotification($"Heartbeat check @ {DateTime.Now}");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
