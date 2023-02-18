using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace comp1640_dotnet.Repositories
{
	public class IdeaRepository : IIdeaRepository
	{
		private readonly ApplicationDbContext dbContext;

		public IdeaRepository(ApplicationDbContext context)
		{
			dbContext = context;
		}

		public async Task<Idea> CreateIdea(Idea idea)
{
			var result = await dbContext.Ideas.AddAsync(idea);
			await dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Idea> GetIdea(string idIdea)
		{
			return dbContext.Ideas.SingleOrDefault(i => i.Id == idIdea);
		}

		public async Task<IEnumerable<Idea>> GetIdeas()
		{
			return await dbContext.Ideas.ToListAsync();
		}

		public async Task<Idea> RemoveIdea(string idIdea)
		{
			var result = await dbContext.Ideas
							 .SingleOrDefaultAsync(e => e.Id == idIdea);

			if (result != null)
			{
				dbContext.Ideas.Remove(result);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
