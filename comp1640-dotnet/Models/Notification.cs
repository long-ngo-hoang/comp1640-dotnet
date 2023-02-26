using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Notification
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[ForeignKey("User")]
		public string? UserId { get; set; }
		public User? User { get; set; }

		[ForeignKey("Idea")]
		public string? IdeaId { get; set; }
		public Idea? Idea { get; set; }

		[ForeignKey("Comment")]
		public string? CommentId { get; set; }
		public Comment? Comment { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string Description { get; set; } = string.Empty;
	}
}
