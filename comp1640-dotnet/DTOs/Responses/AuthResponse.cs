using comp1640_dotnet.Models;

namespace comp1640_dotnet.DTOs.Responses
{
	public class AuthResponse
	{
		public string Token { get; set; } = string.Empty;
		public string? RefreshToken { get; set; }
	}
}