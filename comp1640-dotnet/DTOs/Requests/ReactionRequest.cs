using comp1640_dotnet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.DTOs.Requests
{
	public class ReactionRequest
	{
		public string UserId { get; set; } = string.Empty;
		public string IdeaId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;

	}
}