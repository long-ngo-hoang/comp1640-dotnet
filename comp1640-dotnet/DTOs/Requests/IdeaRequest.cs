namespace comp1640_dotnet.DTOs.Requests
{
	public class IdeaRequest
	{
		public string AcademicYearId { get; set; } = string.Empty;

		public string DepartmentId { get; set; } = string.Empty;

		public string UserId { get; set; } = string.Empty;

		public string CategoryId { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public bool IsAnonymous { get; set; }
	}
}
