﻿using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using comp1640_dotnet.Services;
using comp1640_dotnet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IEmailService _emailService;
		private readonly INotificationRepository _notificationRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CommentRepository(ApplicationDbContext dbContext,
			IEmailService emailService,
			INotificationRepository notificationRepository,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_emailService = emailService;
			_notificationRepository = notificationRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<CommentResponse> CreateComment(CommentRequest comment)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			Comment commentToCreate = new() {
				IdeaId = comment.IdeaId,
				UserId = userId,
				Content = comment.Content,
				IsAnonymous = comment.IsAnonymous
			};

			var ideaInDb = _dbContext.Ideas
				.Include(u => u.User)
				.SingleOrDefault(i => i.Id == comment.IdeaId);

			var author = _dbContext.Profiles.SingleOrDefault(p => p.UserId == userId);

			var result = await _dbContext.Comments.AddAsync(commentToCreate);
			await _dbContext.SaveChangesAsync();

			CommentResponse commentResponse = new()
			{
				Id = result.Entity.Id,
				IdeaId = result.Entity.IdeaId,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Content = result.Entity.Content,
				IsAnonymous = result.Entity.IsAnonymous,
				Author = author.FullName,
			};

			_notificationRepository.CreateNotification(ideaInDb.UserId, 
				null, result.Entity.Id, "Your ideas have new comments.");

			_emailService.SendEmail(ideaInDb.User.Email, "Your idea has a new comment.");

			return commentResponse;
		}

		public async Task<Comment?> RemoveComment(string idComment)
		{
			var result = await _dbContext.Comments
							 .SingleOrDefaultAsync(e => e.Id == idComment);

			if(result == null)
			{
				return null;
			}

			_dbContext.Comments.Remove(result);
			await _dbContext.SaveChangesAsync();
			
			return result;
		}

		public async Task<CommentResponse?> UpdateComment(string idComment, CommentRequest comment)
		{
			CommentResponse? commentResponse = new();

			var commentInDb = await _dbContext.Comments
							 .SingleOrDefaultAsync(e => e.Id == idComment);

			if (commentInDb == null)
			{
				return null;
			}

			commentInDb.Content = comment.Content;
			commentInDb.IsAnonymous = comment.IsAnonymous;

			await _dbContext.SaveChangesAsync();
			commentResponse.Id = commentInDb.Id;
			commentResponse.IdeaId = commentInDb.IdeaId;
			commentResponse.CreatedAt = commentInDb.CreatedAt;
			commentResponse.UpdatedAt = commentInDb.UpdatedAt;
			commentResponse.Content = commentInDb.Content;
			commentResponse.IsAnonymous = commentInDb.IsAnonymous;
			commentResponse.Author = commentInDb.Content;

			return commentResponse;
		}
	}
}
