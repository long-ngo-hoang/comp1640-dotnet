using comp1640_dotnet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.DTOs.Responses
{
	public class IdeaResponse
	{
		public string Id { get; set; } = string.Empty;

		public string AcademicYearId { get; set; } = string.Empty;

		public string UserId { get; set; } = string.Empty;

		public string CategoryId { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public bool IsAnonymous { get; set; }

		public List<ReactionResponse>? Reactions { get; set; }
		public List<Comment>? Comments { get; set; }
		public List<DocumentResponse>? Documents { get; set; }
	}
}
