using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Repositories
{
	public class IdeaRepository : IIdeaRepository
	{
		private readonly ApplicationDbContext dbContext;

		public IdeaRepository(ApplicationDbContext context)
		{
			dbContext = context;
		}

		public async Task<Idea> GetIdea(string idIdea)
		{
			return dbContext.Ideas.SingleOrDefault(i => i.Id == idIdea);
		}

		public async Task<IEnumerable<Idea>> GetIdeas()
		{
			return await dbContext.Ideas.ToListAsync();
		}
	}
}
