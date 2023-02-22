using Amazon.S3.Model;
using Amazon.S3;
using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.DTOs.Requests;

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
		public async Task<ActionResult<IEnumerable<IdeaResponse>>> GetIdeas()
		{
			var result = await ideaRepos.GetIdeas();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<IdeaResponse>> GetIdea(string id)
		{
			var result = await ideaRepos.GetIdea(id);
			if(result == null)
			{
				return BadRequest("Idea not found");
			}
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<IdeaResponse>> CreateIdea(IdeaRequest idea)
		{
			var result = await ideaRepos.CreateIdea(idea);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Idea>> RemoveIdea(string id)
		{
			var result = await ideaRepos.RemoveIdea(id);
			if(result == null)
			{
				return BadRequest("Idea not found");
			}
			return Ok();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<IdeaResponse>> UpdateIdea(string id, IdeaRequest idea)
		{
			var result = await ideaRepos.UpdateIdea(id, idea);
			if (result == null)
			{
				return BadRequest("Idea not found");
			}
			return Ok(result);
		}

		[HttpGet("GetS3PreSignedUrl")]
		public async Task<ActionResult<PreSignedUrlResponse>> GetS3PreSignedUrl()
		{
			var result = ideaRepos.GetS3PreSignedUrl();

			return result;
		}
	}
}
