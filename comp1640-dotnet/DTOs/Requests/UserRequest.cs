using System.ComponentModel.DataAnnotations;

namespace comp1640_dotnet.DTOs.Requests
{
	public class UserRequest
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = string.Empty ;
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
		[Required]
		public string DepartmentId { get; set; } = string.Empty;
	}
}
 