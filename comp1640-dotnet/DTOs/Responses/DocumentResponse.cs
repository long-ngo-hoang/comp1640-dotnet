using comp1640_dotnet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.DTOs.Responses
{
	public class DocumentResponse
	{
		public string Id { get; set; } = string.Empty;

		public string IdeaId { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } 
		public DateTime UpdatedAt { get; set; } 

		public string DocumentUrl { get; set; } = string.Empty;


	}
}
