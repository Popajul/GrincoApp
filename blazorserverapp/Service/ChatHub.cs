using grincoAppModels;
using Microsoft.AspNetCore.SignalR;

namespace blazorserverapp.Service
{
    public class ChatHub : Hub
    {
         public async Task SendMessage(Message message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
    }
}