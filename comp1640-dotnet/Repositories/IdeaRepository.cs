﻿using Amazon.S3.Model;
using Amazon.S3;
using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Amazon;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.Runtime;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.DTOs.Requests;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace comp1640_dotnet.Repositories
{
	public class IdeaRepository : IIdeaRepository
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IConfiguration configuration;
		private static readonly int pageSize = 5;


		public IdeaRepository(ApplicationDbContext _context, IConfiguration _configuration)
		{
			dbContext = _context;
			configuration = _configuration;
		}

		public async Task<IdeaResponse> CreateIdea(IdeaRequest idea)
		{
			Idea ideaToCreate = new()
			{
				AcademicYearId = idea.AcademicYearId,
				DepartmentId = idea.DepartmentId,
				UserId = idea.UserId,
				CategoryId = idea.CategoryId,
				Name = idea.Name,
				Description = idea.Description,
				IsAnonymous = idea.IsAnonymous
			};

			var result = await dbContext.Ideas.AddAsync(ideaToCreate);
			await dbContext.SaveChangesAsync();

			IdeaResponse ideaResponse = new()
			{
				Id = result.Entity.Id,
				AcademicYearId = result.Entity.AcademicYearId,
				DepartmentId = idea.DepartmentId,
				UserId = result.Entity.UserId,
				CategoryId = result.Entity.CategoryId,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Name = result.Entity.Name,
				Description = result.Entity.Description,
				IsAnonymous = result.Entity.IsAnonymous
			};

			return ideaResponse;
		}

		public async Task<IdeaResponse> GetIdea(string idIdea)
		{
			var ideaInDb = dbContext.Ideas
				.Include(i => i.Reactions)
				.Include(i => i.Comments)
				.Include(i => i.Documents).SingleOrDefault(i => i.Id == idIdea);

			IdeaResponse ideaResponse = new()
			{
				Id = ideaInDb.Id,
				AcademicYearId = ideaInDb.AcademicYearId,
				DepartmentId = ideaInDb.DepartmentId,
				UserId = ideaInDb.UserId,
				CategoryId = ideaInDb.CategoryId,
				CreatedAt = ideaInDb.CreatedAt,
				UpdatedAt = ideaInDb.UpdatedAt,
				Name = ideaInDb.Name,
				Description = ideaInDb.Description,
				IsAnonymous = ideaInDb.IsAnonymous,
				Reactions = ConvertReactions(ideaInDb.Reactions),
				Comments = ideaInDb.Comments,
				Documents = ConvertDocuments(ideaInDb.Documents),
			};
			return ideaResponse;
		}

		public async Task<AllIdeasResponse> GetIdeas(int pageIndex, string? nameIdea)
		{
			var ideasInDb = new List<Idea>();

			if (nameIdea != null)
			{
				ideasInDb = await dbContext.Ideas.Skip((pageIndex - 1) * pageSize).Take(pageSize)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .Include(i => i.Documents).Where(i => i.Name.Contains(nameIdea)).ToListAsync();
			}
			else
			{
				ideasInDb = await dbContext.Ideas.Skip((pageIndex - 1) * pageSize).Take(pageSize)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .Include(i => i.Documents).ToListAsync();
			}

			AllIdeasResponse allIdeasResponse = new()
			{
				PageIndex = pageIndex,
				TotalPage = (int)Math.Ceiling((double)dbContext.Ideas.Count() / pageSize),
				Ideas = ConvertIdeas(ideasInDb)
			};

			return allIdeasResponse;
		}

		public async Task<Idea> RemoveIdea(string idIdea)
		{
			var result = await dbContext.Ideas
							 .SingleOrDefaultAsync(e => e.Id == idIdea);

			if (result != null)
			{
				dbContext.Ideas.Remove(result);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}

		public async Task<IdeaResponse?> UpdateIdea(string idIdea, IdeaRequest idea)
		{
			var ideaInDb = await dbContext.Ideas
				.Include(i => i.Reactions)
				.Include(i => i.Comments)
				.Include(i => i.Documents)
				.SingleOrDefaultAsync(e => e.Id == idIdea);

			IdeaResponse? ideaResponse = new();


			if (ideaInDb == null)
			{
				return null;
			}
			else
			{
				ideaInDb.Name = idea.Name;
				ideaInDb.Description = idea.Description;
				ideaInDb.IsAnonymous = idea.IsAnonymous;
				await dbContext.SaveChangesAsync();

				ideaResponse.Id = ideaInDb.Id;
				ideaResponse.AcademicYearId = ideaInDb.AcademicYearId;
				ideaResponse.DepartmentId = ideaInDb.DepartmentId;
				ideaResponse.UserId = ideaInDb.UserId;
				ideaResponse.CategoryId = ideaInDb.CategoryId;
				ideaResponse.CreatedAt = ideaInDb.CreatedAt;
				ideaResponse.UpdatedAt = ideaInDb.UpdatedAt;
				ideaResponse.Name = ideaInDb.Name;
				ideaResponse.Description = ideaInDb.Description;
				ideaResponse.IsAnonymous = ideaInDb.IsAnonymous;
				//ideaResponse.Reactions = ideaInDb.Reactions;
				ideaResponse.Comments = ideaInDb.Comments;
				ideaInDb.Documents = ideaInDb.Documents;
			}

			return ideaResponse;
		}

		public PreSignedUrlResponse GetS3PreSignedUrl()
		{
			var amazonAccessKey = configuration["AWS:ACCESS_KEY"];
			var amazonSecretKey = configuration["AWS:SECRET_KEY"];
			var amazonBucketName = configuration["AWS:BUCKET_NAME"];
			string fileName = Guid.NewGuid().ToString();

			IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1);

			GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
			{
				BucketName = amazonBucketName,
				Key = fileName + ".jpg",
				Expires = DateTime.Now.AddMinutes(20),
				Verb = HttpVerb.PUT,
			};

			string preSignedUrl = client.GetPreSignedURL(request);

			PreSignedUrlResponse preSignedUrlResponse = new PreSignedUrlResponse
			{
				FileName = fileName + ".jpg",
				PreSignedUrl = preSignedUrl,
			};
			return preSignedUrlResponse;
		}

		private static List<ReactionResponse> ConvertReactions(List<Reaction> _reactions)
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

		private static List<DocumentResponse> ConvertDocuments(List<Document> _documents)
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

		private static List<IdeaResponse> ConvertIdeas(List<Idea> _ideas)
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
					Reactions = ConvertReactions(x.Reactions),
					Documents = ConvertDocuments(x.Documents)

				}).ToList();

			return ideas;
		}
	}
}