﻿using comp1640_dotnet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp1640_dotnet.DTOs.Responses
{
	public class IdeaResponse
	{
		public string? Id { get; set; }

		public string? CategoryId { get; set; } 

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string? Name { get; set; }
		public string? Description { get; set; }
		public bool IsAnonymous { get; set; }
		public int ViewCount { get; set; }
		public string? Author { get; set; }

		public List<ReactionResponse>? Reactions { get; set; }
		public List<CommentResponse>? Comments { get; set; }
		public List<DocumentResponse>? Documents { get; set; }
	}
}
