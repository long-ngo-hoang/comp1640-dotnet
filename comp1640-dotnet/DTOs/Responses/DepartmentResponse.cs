namespace comp1640_dotnet.DTOs.Responses
{
	public class DepartmentResponse
	{
		public string Id { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string Name { get; set; } = string.Empty;

		public AllIdeasResponse? AllIdeas { get; set; }
	}
}