using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Comment
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		[ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }

		[ForeignKey("Idea")]
		public Guid IdeaId { get; set; }
		public Idea? Idea { get; set; }

		public DateTime CreatedAt { get; set;	} = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string? Content { get; set; }
		public bool IsAnonymous { get; set; }
	}
}
