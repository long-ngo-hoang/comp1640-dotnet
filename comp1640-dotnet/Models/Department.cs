using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace comp1640_dotnet.Models
{
	public class Department
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		 
		public string Name { get; set; } = string.Empty;

		//relation
		public List<User>? Users { get; set; }
		public List<Idea>? Ideas { get; set; }
	}
}
