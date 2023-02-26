using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Invitation
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[ForeignKey("User")]
		public string? UserId { get; set; }
		public User? User { get; set; }

		[ForeignKey("InviteUser")]
		public string? InviteUserId { get; set; }
		public User? InviteUser { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string Description { get; set; } = string.Empty;
	}
}
