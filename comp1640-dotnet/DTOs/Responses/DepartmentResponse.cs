namespace comp1640_dotnet.DTOs.Responses
{
	public class DepartmentResponse
	{
		public string Id { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string Name { get; set; } = string.Empty;
		public int TotalIdeas { get; set; }
		public AllIdeasResponse? AllIdeas { get; set; }
		public List<UserResponse>? AllUsers { get; set; }
	}
}