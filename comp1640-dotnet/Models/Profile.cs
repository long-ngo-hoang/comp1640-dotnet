using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace comp1640_dotnet.Models
{
	public class Profile
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		//relations
		[ForeignKey("User")]
		public string? UserId { get; set; }
		public User? User { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string? AvatarUrl { get; set; }
		public string? FullName { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }




	}
}
