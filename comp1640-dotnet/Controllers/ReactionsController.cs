using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class ReactionsController : ControllerBase
	{
		private readonly IReactionRepository _reactionRepos;
		private readonly IIdeaRepository _ideaRepos;

		public ReactionsController(IReactionRepository reactionRepos, IIdeaRepository ideaRepos)
		{
			_reactionRepos = reactionRepos;
			_ideaRepos = ideaRepos;
		}

		[HttpPost]
		public async Task<ActionResult<ReactionResponse>> CreateReaction(ReactionRequest reaction)
		{
			var ideaInDb = _ideaRepos.IdeaExistsInDb(reaction.IdeaId);
			if(ideaInDb == null)
			{
				return BadRequest("Idea not found");
			}
			var result = await _reactionRepos.CreateReaction(reaction);
			if(result != null)
			{
				return Ok(result);
			}
			return BadRequest("Can't create response now.");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Reaction>> RemoveReaction(string id)
		{
			var result = await _reactionRepos.RemoveReaction(id);
			if(result == null)
			{
				return BadRequest("Reaction not found");
			}
			return Ok("Delete successful reaction");
		}
	}
}
