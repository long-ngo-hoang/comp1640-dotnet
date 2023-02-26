using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class UserRole
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[ForeignKey("User")]
		public string? UserId { get; set; }
		public User? User { get; set; }

		[ForeignKey("Role")]
		public string? RoleId { get; set; }
		public Role? Role { get; set; }
	}
}
