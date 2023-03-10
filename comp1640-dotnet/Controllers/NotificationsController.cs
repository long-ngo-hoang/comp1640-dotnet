using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class NotificationsController : ControllerBase
	{
		private readonly INotificationRepository notificationRepos;

		public NotificationsController(INotificationRepository _notificationRepos)
		{
			notificationRepos = _notificationRepos;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
		{
			var result = await notificationRepos.GetNotifications();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<NotificationResponse>> GetNotification(string id)
		{
			var result = await notificationRepos.GetNotification(id);
			if(result == null)
			{
				return BadRequest("Notifications not found");
			}
			return Ok(result);
		}
	}
}
