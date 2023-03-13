
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.Repositories;
using comp1640_dotnet.Services.Interfaces;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class IdeasController : ControllerBase
	{
		private readonly IIdeaRepository _ideaRepos;		
		private readonly IAcademicYearRepository _academicYearRepos;
		private readonly INotificationRepository _notificationRepos;
		private readonly IUserRepository _userRepos;
		private readonly IEmailService _emailService;
		private readonly IS3Service _s3Service;

		public IdeasController(IIdeaRepository ideaRepos,
			IAcademicYearRepository academicYearRepos,
			INotificationRepository notificationRepos,
			IUserRepository userRepos,
			IEmailService emailService,
			IS3Service s3Service)
		{
			_ideaRepos = ideaRepos;
			_academicYearRepos = academicYearRepos;
			_notificationRepos = notificationRepos;
			_userRepos = userRepos;
			_emailService = emailService;
			_s3Service = s3Service;
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
			
			if(result == null)
			{
				return BadRequest("Ideas not found");
			}

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

			if(result == null)
			{
				return BadRequest("Can't create idea now.");
			}

			User qACoordinator = await _userRepos.GetQACoordinator();

			_notificationRepos.CreateNotification(qACoordinator.Id, result.Id,
						null, "The staff in the department just created a new idea.");

		  _emailService.SendEmail(qACoordinator.Email, "Your employee just posted an idea", "< a href = '' > Click </ a > ");

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

		[HttpGet("GetPreSignedUrlToUploadDocument")]
		public async Task<ActionResult<PreSignedUrlResponse>> GetS3PreSignedUrl()
		{
			var result = await _s3Service.GetPreSignedUrl("documents/");

			return result;
		}

		[HttpGet("GetMostPopularIdeas")]
		public async Task<ActionResult<AllIdeasResponse>> GetMostPopularIdeas(int pageIndex = 1)
		{
			var result = await _ideaRepos.GetMostPopularIdeas(pageIndex);

			return result;
		}		
		
		[HttpGet("GetMostViewedIdeas")]
		public async Task<ActionResult<AllIdeasResponse>> GetMostViewedIdeas(int pageIndex = 1)
		{
			var result = await _ideaRepos.GetMostViewedIdeas(pageIndex);

			return result;
		}
	}
}
