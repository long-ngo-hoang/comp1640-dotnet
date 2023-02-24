using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.Models
{
	public class RefreshToken
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();

		//relations
		[ForeignKey("User")]
		public string UserId { get; set; } = string.Empty;
		public User? User { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public string Token { get; set; } = String.Empty;
		public DateTime TokenCreated { get; set; } = DateTime.Now;
		public DateTime TokenExpires { get; set; } = DateTime.Now;
	}
}
