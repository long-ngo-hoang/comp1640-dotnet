using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Comment
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		//relations
		[ForeignKey("User")]
		public string? UserId { get; set; }
		public User? User { get; set; }

		[ForeignKey("Idea")]
		public string? IdeaId { get; set; }
		public Idea? Idea { get; set; }

		public DateTime CreatedAt { get; set;	} = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string Content { get; set; } = string.Empty;
		public bool IsAnonymous { get; set; }	
		

	}
}
