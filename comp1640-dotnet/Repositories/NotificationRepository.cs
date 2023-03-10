using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public NotificationRepository(ApplicationDbContext dbContext,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		public async void CreateNotification(string userId, string? ideaId, string? commentId, string description)
		{
			Notification notificationToCreate = new()
			{
				UserId = userId,
				IdeaId = ideaId,
				CommentId = commentId,
				Description = description,
				IsRead = false
			};
			await _dbContext.Notifications.AddAsync(notificationToCreate);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<NotificationResponse>? GetNotification(string idNotification)
		{
			var notificationInDb = _dbContext.Notifications
				.Include(i => i.Idea)
				.Include(c => c.Comment)
				.SingleOrDefault(n => n.Id == idNotification);

			if(notificationInDb == null)
			{
				return null;
			}

			notificationInDb.IsRead = true;
			await _dbContext.SaveChangesAsync();

			NotificationResponse notificationResponse = new()
			{
				Id = notificationInDb.Id,
				IdeaId = notificationInDb.IdeaId,
				Idea = notificationInDb.Idea,
				CommentId = notificationInDb.CommentId,
				Comment = notificationInDb.Comment,
				CreatedAt = notificationInDb.CreatedAt,
				UpdatedAt = notificationInDb.UpdatedAt,
				Description = notificationInDb.Description
			};

			return notificationResponse;
		}

		public async Task<IEnumerable<Notification>> GetNotifications()
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var notificationsInDb = await _dbContext.Notifications
				.Where(n => n.UserId == userId && n.IsRead == false)
				.ToListAsync();

			return notificationsInDb;
		}
	}
}
