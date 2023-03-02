namespace comp1640_dotnet.DTOs.Responses
{
	public class ProfileResponse
	{
		public string Id { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string? AvatarUrl { get; set; }
		public string? FullName { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }
	}
}