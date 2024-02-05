using Microsoft.AspNetCore.SignalR;

namespace Mvc.Hubs
{
	public class CustomUserIdProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			var userId = connection.User.Identity.Name;
			return userId.ToString();
		}
	}
}
