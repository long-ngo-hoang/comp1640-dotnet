using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<User> GetQACoordinator();	
		Task<User> GetAuthor(string ideaId);	
	}
}
