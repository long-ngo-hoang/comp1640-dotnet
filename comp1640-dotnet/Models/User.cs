using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class User
	{
		[Key]
		public string Id { get; set;} = Guid.NewGuid().ToString();
		
		//relations
		public Profile? Profile { get; set; }
		[ForeignKey("Department")]
		public string? DepartmentId { get; set; }
		public Department? Department { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string? UserName { get; set; }
		public string? Email { get; set; }
		public byte[]? Password { get; set; }

		//relations
		public List<Reaction>? Reactions { get; set; }
		public List<Comment>? Comments	{ get; set; }
		public List<Idea>? Ideas { get; set; }

	}
}

