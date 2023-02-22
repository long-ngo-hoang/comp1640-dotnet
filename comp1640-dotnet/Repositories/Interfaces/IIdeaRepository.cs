using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IIdeaRepository
	{
		Task<IEnumerable<Idea>> GetIdeas();
		Task<IdeaResponse> GetIdea(string idIdea);
		Task<IdeaResponse> CreateIdea(IdeaRequest idea);
		Task<Idea> RemoveIdea (string idIdea);
		Task<IdeaResponse> UpdateIdea(string idIdea, IdeaRequest idea);
		PreSignedUrlResponse GetS3PreSignedUrl();

	}
}
