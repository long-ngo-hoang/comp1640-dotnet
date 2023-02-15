using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace comp1640_dotnet.Models
{
	public class Category
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		
		public DateTime CreatedAt { get; }
		public DateTime UpdatedAt { get; }

		public string? Name { get; set; }

		//relations
		public List<Idea>? Ideas { get; set; }
	}
}
