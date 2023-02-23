using comp1640_dotnet.Models;

namespace comp1640_dotnet.DTOs.Responses
{
	public class AllIdeasResponse
	{
		public int PageIndex { get; set; }
		public int TotalPage { get; set; }
		public List<Idea>? Ideas { get; set; }
	}
}
