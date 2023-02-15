using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Invitation
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		[ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }

		[ForeignKey("ApplicationUser")]
		public string? InvitedUserId { get; set; }
		public ApplicationUser? InvitedUser { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public bool IsAccepted { get; set; }
	}
}
