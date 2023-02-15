using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace comp1640_dotnet.Models
{
	public class Department
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		public DateTime CreatedAt { get; }
		public DateTime UpdatedAt { get; }

		public string? Name { get; set; }

		//relation
		public List<ApplicationUser>? Users { get; set; }
	}
}
