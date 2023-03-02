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
	[Authorize]
	public class CommentsController : ControllerBase
	{
		private readonly ICommentRepository documentRepos;

		public CommentsController(ICommentRepository _documentRepos)
		{
			this.documentRepos = _documentRepos;
		}

		[HttpPost]
		public async Task<ActionResult<CommentResponse>> CreateComment(CommentRequest comment)
		{
			var result = await documentRepos.CreateComment(comment);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Comment>> RemoveComment(string id)
		{
			var result = await documentRepos.RemoveComment(id);
			if(result == null)
			{
				return BadRequest("Comment not found");
			}
			return Ok("Delete successful comment");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<CommentResponse>> UpdateComment(string id, CommentRequest comment)
		{
			var result = await documentRepos.UpdateComment(id, comment);
			if (result == null)
			{
				return BadRequest("Comment not found");
			}
			return Ok("Update successful comment");
		}
	}
}
