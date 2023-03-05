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
	public class CommentsController : ControllerBase
	{
		private readonly ICommentRepository _documentRepos;
		private readonly IAcademicYearRepository _academicYearRepos;
		private readonly IIdeaRepository _ideaRepos;

		public CommentsController(ICommentRepository documentRepos,
			IIdeaRepository ideaRepos,
			IAcademicYearRepository academicYearRepos)
		{
			_documentRepos = documentRepos;
			_ideaRepos = ideaRepos;
			_academicYearRepos = academicYearRepos;
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
