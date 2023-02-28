namespace comp1640_dotnet.DTOs.Requests
{
	public class AcademicYearRequest
	{
		public string Name { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime ClosureDate { get; set; }
		public DateTime FinalClosureDate { get; set; }
	}
}