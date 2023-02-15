using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class ApplicationUser : IdentityUser
	{
		[ForeignKey("Department")]
		public Guid DepartmentId { get; set; }
		public Department? Department { get; set; }
			
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string? Address { get; set; }
		public string? AvatarUrl { get; set; }
		
		//relations
		public List<Reaction>? Reactions { get; set; }
		public List<Comment>? Comments	{ get; set; }
		public List<Idea>? Ideas { get; set; }

	}
}

