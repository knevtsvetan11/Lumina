using ApiLumina.Area.Identity.Services;
using ApiLumina.Area.Identity.Services.Interfaces;
using Lumina.Data;
using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ApiLumina.Hubs;

public class SupportChatHub() : Hub
{
    
    public override async Task OnConnectedAsync()
     {
      

        var connectionId = Context.ConnectionId;
        await base.OnConnectedAsync();
    }

    public async Task JoinRoom(string userId)
    {

        await Groups.AddToGroupAsync(Context.ConnectionId, userId);

       
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {

    
        await base.OnDisconnectedAsync(exception);
    }



}
