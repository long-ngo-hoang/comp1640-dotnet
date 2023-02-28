using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface INotificationRepository
	{
		Task<IEnumerable<Notification>> GetNotifications(string userId);
		Task<NotificationResponse> GetNotification(string idNotification);
		void CreateNotification(string userId, string? ideaId, string? commentId, string description);
	}
}
