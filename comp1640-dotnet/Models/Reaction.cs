﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Reaction
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		[ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }

		[ForeignKey("Idea")]
		public Guid IdeaId { get; set; }
		public Idea? Idea { get; set; }

		public DateTime CreatedAt { get; }
		public DateTime UpdatedAt { get; }

		public string? Name { get; set; }

	}
}
