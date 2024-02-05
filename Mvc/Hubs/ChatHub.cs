using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Mvc.Hubs
{
	public class ChatHub : Hub
	{
        public async Task SendMessage(string userId, string message)
        {
            await Clients.Users(userId).SendAsync("ReceiveMessage", message);
        }

        //public string GetConnectionId() => Context.ConnectionId;
    }

}
