using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IReactionRepository
	{
		Task<ReactionResponse> CreateReaction(ReactionRequest Reaction);
		Task<Reaction> RemoveReaction (string ideaId);
		Task<Reaction> ReactionExistsInDb(string idIdea);

	}
}
