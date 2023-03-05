
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.DTOs.Requests;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class IdeasController : ControllerBase
	{
		private readonly IIdeaRepository _ideaRepos;		
		private readonly IAcademicYearRepository _academicYearRepos;

		public IdeasController(IIdeaRepository ideaRepos,
			IAcademicYearRepository academicYearRepos)
		{
			_ideaRepos = ideaRepos;
			_academicYearRepos = academicYearRepos;
		}

		[HttpGet]
		public async Task<ActionResult<AllIdeasResponse>> GetIdeas(int pageIndex = 1, string? nameIdea = null)
		{
			var result = await _ideaRepos.GetIdeas(pageIndex, nameIdea);
			return Ok(result);
		}

		[HttpGet, Route("UserId")]
		public async Task<ActionResult<AllIdeasResponse>> GetIdeasByUserId(int pageIndex = 1, string? nameIdea = null)
		{
			var result = await _ideaRepos.GetIdeasByUserId(pageIndex, nameIdea);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<IdeaResponse>> GetIdea(string id)
		{
			var result = await _ideaRepos.GetIdea(id);
			if(result == null)
			{
				return BadRequest("Idea not found");
			}
			return Ok(result);
		}

		[HttpPost, Authorize(Roles = "Staff")]
		public async Task<ActionResult<IdeaResponse>> CreateIdea(IdeaRequest idea)
		{
		 var deadlineForNewIdeas = await _academicYearRepos.CheckDeadlineForNewIdeas();
			
			if (deadlineForNewIdeas != true)
			{
				return BadRequest("The deadline for creating new ideas has expired.");
			}

				var result = await _ideaRepos.CreateIdea(idea);
				return Ok(result);
		}

		[HttpDelete("{id}"), Authorize(Roles = "Staff")]
		public async Task<ActionResult<Idea>> RemoveIdea(string id)
		{
			var result = await _ideaRepos.RemoveIdea(id);
			if(result == null)
			{
				return BadRequest("Idea not found");
			}
			return Ok();
		}

		[HttpPut("{id}"), Authorize(Roles = "Staff")]
		public async Task<ActionResult<IdeaResponse>> UpdateIdea(string id, IdeaRequest idea)
		{		
			var result = await _ideaRepos.UpdateIdea(id, idea);
			if (result == null)
			{
				return BadRequest("Idea not found");
			}
			return Ok(result);
		}

		[HttpGet("GetS3PreSignedUrl")]
		public async Task<ActionResult<PreSignedUrlResponse>> GetS3PreSignedUrl()
		{
			var result = _ideaRepos.GetS3PreSignedUrl();

			return result;
		}
	}
}
