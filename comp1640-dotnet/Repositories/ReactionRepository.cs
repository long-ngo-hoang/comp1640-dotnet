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

		public async Task<ReactionResponse?> CreateReaction(ReactionRequest reaction)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			Reaction reactionToCreate = new()
			{
				IdeaId = reaction.IdeaId,
				UserId = userId,
				Name = reaction.Name
			};

			var result = await _dbContext.Reactions.AddAsync(reactionToCreate);
			await _dbContext.SaveChangesAsync();

			if(result == null)
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

		public async Task<Reaction> RemoveReaction(string idReaction)
		{
			var result = await _dbContext.Reactions
							 .SingleOrDefaultAsync(e => e.Id == idReaction);

			if (result != null)
			{
				_dbContext.Reactions.Remove(result);
				await _dbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}
