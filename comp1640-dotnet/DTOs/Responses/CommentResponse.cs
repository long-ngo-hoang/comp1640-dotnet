namespace comp1640_dotnet.DTOs.Responses
{
	public class CommentResponse
	{
		public string Id { get; set; } = string.Empty;
		
		public string IdeaId { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string Content { get; set; } = string.Empty;
		public bool IsAnonymous { get; set; }
		public string Author { get; set; } = string.Empty;
	}
}