using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IProfileRepository
	{
		Task<ProfileResponse> GetProfile();
		Task<ProfileResponse> GetProfileByUserId(string userId);
		Task<Profile> CreateProfile(string userId);
		Task<ProfileResponse> UpdateProfileByUserId(string userId, ProfileRequest profile);
	}
}
