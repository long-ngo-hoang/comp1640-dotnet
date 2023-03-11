using comp1640_dotnet.DTOs.Responses;

namespace comp1640_dotnet.Services.Interfaces
{
	public interface IS3Service
	{
		Task<PreSignedUrlResponse> GetPreSignedUrl(string path);
		
	}
}
