using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IUserRoleRepository
	{
		Task<UserRole> GetUserRole(string userId);
		Task<UserRole> UpdateUserRole(string userId, string roleId);
	}
}
