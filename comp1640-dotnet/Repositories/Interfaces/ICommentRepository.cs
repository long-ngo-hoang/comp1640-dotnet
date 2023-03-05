using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface ICommentRepository
	{
		Task<CommentResponse> CreateComment(CommentRequest comment, Idea idea);	
		Task<CommentResponse> UpdateComment(string idComment, CommentRequest comment);
		Task<Comment> RemoveComment(string idComment);
	}
}
