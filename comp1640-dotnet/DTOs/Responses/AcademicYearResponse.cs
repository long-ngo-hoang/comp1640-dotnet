using comp1640_dotnet.Models;

namespace comp1640_dotnet.DTOs.Responses
{
	public class AcademicYearResponse
	{
		public string Id { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string Name { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime ClosureDate { get; set; }
		public DateTime FinalClosureDate { get; set; }

		public AllIdeasResponse? AllIdeas { get; set; }
	}
}