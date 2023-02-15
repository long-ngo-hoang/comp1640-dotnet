using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Document
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[ForeignKey("Idea")]
		public Guid IdeaId { get; set; }
		public Idea? Idea { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string? DocumentUrl { get; set; }
	}
}
