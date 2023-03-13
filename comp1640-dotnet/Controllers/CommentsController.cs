using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories;
using comp1640_dotnet.Repositories.Interfaces;
using comp1640_dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class CommentsController : ControllerBase
	{
		private readonly ICommentRepository _documentRepos;
		private readonly IAcademicYearRepository _academicYearRepos;
		private readonly IIdeaRepository _ideaRepos;
		private readonly INotificationRepository _notificationRepos;
		private readonly IUserRepository _userRepos;
		private readonly IEmailService _emailService;

		public CommentsController(ICommentRepository documentRepos,
			IIdeaRepository ideaRepos,
			IAcademicYearRepository academicYearRepos,
			INotificationRepository notificationRepos,
			IUserRepository userRepos,
			IEmailService emailService)
		{
			_documentRepos = documentRepos;
			_ideaRepos = ideaRepos;
			_academicYearRepos = academicYearRepos;
			_notificationRepos = notificationRepos;
			_userRepos = userRepos;
			_emailService = emailService;
		}

		[HttpPost]
		public async Task<ActionResult<CommentResponse>> CreateComment(CommentRequest comment)
		{
			var deadlineForNewComments = await _academicYearRepos.CheckDeadlineForNewComments();

			var ideaInDb = await _ideaRepos.IdeaExistsInDb(comment.IdeaId);

			if(ideaInDb == null)
			{
				return BadRequest("Idea not found");
			}

			if (deadlineForNewComments != true)
			{
				return BadRequest("The deadline for creating new comments has expired.");
			}

			var result = await _documentRepos.CreateComment(comment, ideaInDb);

			if (result == null)
			{
				return BadRequest("Can't create comment now.");
			}

			User author = await _userRepos.GetAuthor(ideaInDb.Id);

			_notificationRepos.CreateNotification(author.Id,
				null, result.Id, "Your ideas have new comments.");

		  _emailService.SendEmail(author.Email, "Your idea has a new comment.", "< a href = '' > Click </ a > ");

			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Comment>> RemoveComment(string id)
		{
			var result = await _documentRepos.RemoveComment(id);

			if(result == null)
			{
				return BadRequest("Comment not found");
			}

			return Ok("Delete successful comment");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<CommentResponse>> UpdateComment(string id, CommentRequest comment)
		{
			var result = await _documentRepos.UpdateComment(id, comment);
			if (result == null)
			{
				return BadRequest("Comment not found");
			}
			return Ok("Update successful comment");
		}
	}
}
