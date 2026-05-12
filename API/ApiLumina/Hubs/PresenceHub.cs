using ApiLumina.Area.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using static Lumina.Services.Core.Constants.Permissions;

namespace ApiLumina.Hubs;

[Authorize(AuthenticationSchemes = "Bearer")]
public class PresenceHub(PresenceTracker tracker, IAuthService authService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = authService.GetUserId(Context.User);

        if (await tracker.UserConnected(userId, Context.ConnectionId))
        {
            await Clients.Others.SendAsync("UserIsOnline", userId);
        }

        var currentUsers = await tracker.GetOnlineUsers();
        await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = authService.GetUserId(Context.User);

        if (await tracker.UserDisconnected(userId, Context.ConnectionId))
        {
            await Clients.Others.SendAsync("UserIsOffline", userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}