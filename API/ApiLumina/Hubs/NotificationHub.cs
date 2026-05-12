using Microsoft.AspNetCore.SignalR;

namespace ApiLumina.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        await base.OnConnectedAsync();
    }
}
