using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace comp1640_dotnet.Models
{
	public class AcademicYear
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime ClosureDate { get; set; }
		public DateTime FinalClosureDate { get; set; }

		//relations 
		public List<Idea>? Ideas { get; set; }

	}
}
