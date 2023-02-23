using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ReactionsController : ControllerBase
	{
		private readonly IReactionRepository reactionRepos;

		public ReactionsController(IReactionRepository _reactionRepos)
		{
			this.reactionRepos = _reactionRepos;
		}


		[HttpPost]
		public async Task<ActionResult<ReactionResponse>> CreateReaction(ReactionRequest reaction)
		{
			var result = await reactionRepos.CreateReaction(reaction);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Reaction>> RemoveReaction(string id)
		{
			var result = await reactionRepos.RemoveReaction(id);
			if(result == null)
			{
				return BadRequest("Reaction not found");
			}
			return Ok("Delete successful reaction");
		}
	}
}
