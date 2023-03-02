using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class ReactionRepository : IReactionRepository
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;

		public ReactionRepository(ApplicationDbContext context, IHttpContextAccessor _httpContextAccessor)
		{
			dbContext = context;
			httpContextAccessor = _httpContextAccessor;
		}

		public async Task<ReactionResponse> CreateReaction(ReactionRequest reaction)
		{
			var userId = httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			Reaction reactionToCreate = new()
			{
				IdeaId = reaction.IdeaId,
				UserId = userId,
				Name = reaction.Name
			};

			var result = await dbContext.Reactions.AddAsync(reactionToCreate);
			await dbContext.SaveChangesAsync();

			ReactionResponse reactionResponse = new()
			{
				Id = result.Entity.Id,
				IdeaId = result.Entity.IdeaId,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Name = result.Entity.Name,
				Author = result.Entity.Name
			};
			return reactionResponse;
		}

		public async Task<Reaction> RemoveReaction(string idReaction)
		{
			var result = await dbContext.Reactions
							 .SingleOrDefaultAsync(e => e.Id == idReaction);

			if (result != null)
			{
				dbContext.Reactions.Remove(result);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
