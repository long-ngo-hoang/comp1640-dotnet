using comp1640_dotnet.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace comp1640_dotnet.DTOs.Requests
{
	public class ProfileRequest
	{
		public string? AvatarUrl { get; set; }
		public string? FullName { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }
	}
}