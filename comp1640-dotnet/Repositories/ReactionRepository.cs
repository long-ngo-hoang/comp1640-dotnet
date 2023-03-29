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
		private readonly ApplicationDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ReactionRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ReactionResponse?> UpdateReaction(ReactionRequest reaction)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var reactionInDb = _dbContext.Reactions.SingleOrDefault(i => i.IdeaId == reaction.IdeaId && i.UserId == userId);

			if (reactionInDb != null)
			{
				if(reactionInDb.Name == reaction.Name)
				{
					_dbContext.Reactions.Remove(reactionInDb);
				}
				else
				{
					reactionInDb.Name = reaction.Name;
				}
				await _dbContext.SaveChangesAsync();
				ReactionResponse updateReaction = new()
				{
					Id = reactionInDb.Id,
					IdeaId = reactionInDb.IdeaId,
					CreatedAt = reactionInDb.CreatedAt,
					UpdatedAt = reactionInDb.UpdatedAt,
					Name = reactionInDb.Name,
					Author = reactionInDb.Name
				};
				return updateReaction;
			}
			Reaction reactionToCreate = new()
				{
					IdeaId = reaction.IdeaId,
					UserId = userId,
					Name = reaction.Name
				};
			var result = await _dbContext.Reactions.AddAsync(reactionToCreate);
				await _dbContext.SaveChangesAsync();
				
				if (result == null)
				{
					return null;
				}
				
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
	}
}
