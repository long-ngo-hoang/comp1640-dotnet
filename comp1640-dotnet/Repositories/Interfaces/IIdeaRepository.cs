using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IIdeaRepository
	{
		Task<IEnumerable<Idea>> GetIdeas();
		Task<Idea> GetIdea(string idIdea);
	}
}
