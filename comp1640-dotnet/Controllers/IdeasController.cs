using comp1640_dotnet.Data;
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
	public class IdeasController : ControllerBase
	{
		private readonly IIdeaRepository ideaRepos;

		public IdeasController(IIdeaRepository _ideaRepos)
		{
			this.ideaRepos = _ideaRepos;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Idea>>> GetIdeas()
		{
			var result = await ideaRepos.GetIdeas();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Idea>> GetIdeas(string id)
		{
			var result = await ideaRepos.GetIdea(id);
			if(result == null)
			{
				return BadRequest("Idea not found");
			}
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Idea>> CreateIdea(Idea idea)
		{
			var result = await ideaRepos.CreateIdea(idea);
			return Ok(result);
		}
	}
}
