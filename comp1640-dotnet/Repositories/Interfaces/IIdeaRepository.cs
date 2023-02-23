using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IIdeaRepository
	{
		Task<AllIdeasResponse> GetIdeas(int pageIndex, string? nameIdea);
		Task<IdeaResponse> GetIdea(string idIdea);
		Task<IdeaResponse> CreateIdea(IdeaRequest idea);
		Task<Idea> RemoveIdea (string idIdea);
		Task<IdeaResponse> UpdateIdea(string idIdea, IdeaRequest idea);
		PreSignedUrlResponse GetS3PreSignedUrl();

	}
}
