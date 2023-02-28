using comp1640_dotnet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.DTOs.Responses
{
	public class NotificationResponse
	{
		public string Id { get; set; } = string.Empty;

		public string? IdeaId { get; set; }
		public Idea? Idea { get; set; }

		public string? CommentId { get; set; }
		public Comment? Comment { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string Description { get; set; } = string.Empty;
	}
}