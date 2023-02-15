using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class Idea
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[ForeignKey("AcademicYear")]
		public Guid AcademicYearId { get; set; }
		public AcademicYear? AcademicYear { get; set; }
		
		[ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }



		[ForeignKey("Category")]
		public Guid CategoryId { get; set; }
		public Category? Category { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string? Name { get; set; }
		public bool IsAnonymous { get; set; }

		//relations
		public List<Reaction>? Reactions { get; set; }
		public List<Comment>? Comments { get; set; }
		public List<Document>? Documents { get; set; }
	}
}
