using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Factory
{
	public class ConvertFactory
	{
		public List<IdeaResponse> ConvertListIdeas(List<Idea> _ideas)
		{
			var ideas = _ideas
				.Select(x => new IdeaResponse()
				{
					Id = x.Id,
					AcademicYearId = x.AcademicYearId,
					DepartmentId = x.DepartmentId,
					UserId = x.UserId,
					CategoryId = x.CategoryId,
					CreatedAt = x.CreatedAt,
					UpdatedAt = x.UpdatedAt,
					Name = x.Name,
					Description = x.Description,
					IsAnonymous = x.IsAnonymous,
					Reactions = ConvertListReactions(x.Reactions),
					Comments = ConvertListComments(x.Comments),
					Documents = ConvertListDocuments(x.Documents)
				}).ToList();

			return ideas;
		}

		public List<ReactionResponse>? ConvertListReactions(List<Reaction>? _reactions)
		{
			if (_reactions == null)
			{
				return null;
			}
			else
			{
				var reactions = _reactions
					.Select(x => new ReactionResponse()
					{
						Id = x.Id,
						UserId = x.UserId,
						IdeaId = x.IdeaId,
						CreatedAt = x.CreatedAt,
						UpdatedAt = x.UpdatedAt,
						Name = x.Name
					}).ToList();

				return reactions;
			}
		}

		public List<DocumentResponse>? ConvertListDocuments(List<Document>? _documents)
		{
			if (_documents == null)
			{
				return null;
			}
			else
			{
				var documents = _documents
					.Select(x => new DocumentResponse()
					{
						Id = x.Id,
						IdeaId = x.IdeaId,
						CreatedAt = x.CreatedAt,
						UpdatedAt = x.UpdatedAt,
						DocumentUrl = x.DocumentUrl
					}).ToList();

				return documents;
			}
		}

		public List<CommentResponse>? ConvertListComments(List<Comment>? _comments)
		{
			if (_comments == null)
			{
				return null;
			}
			else
			{
				var comments = _comments
					.Select(x => new CommentResponse()
					{
						Id = x.Id,
						UserId = x.UserId,
						IdeaId = x.IdeaId,
						CreatedAt = x.CreatedAt,
						UpdatedAt = x.UpdatedAt,
						Content = x.Content,
						IsAnonymous = x.IsAnonymous
					}).ToList();

				return comments;
			}
		}
	}
}
