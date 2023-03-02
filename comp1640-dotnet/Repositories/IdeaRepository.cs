using Amazon.S3.Model;
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
using comp1640_dotnet.Factory;
using comp1640_dotnet.Services.Interfaces;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class IdeaRepository : IIdeaRepository
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IConfiguration configuration;
		private static readonly int pageSize = 5;
		private readonly ConvertFactory convertFactory;
		private readonly IEmailService emailService;
		private readonly INotificationRepository notificationRepository;
		private readonly IHttpContextAccessor httpContextAccessor;

		public IdeaRepository(ApplicationDbContext _context, 
			IConfiguration _configuration,
			ConvertFactory _convertFactory,
			IEmailService _emailService,
			INotificationRepository _notificationRepository,
			IHttpContextAccessor _httpContextAccessor)
		{
			dbContext = _context;
			configuration = _configuration;
			convertFactory = _convertFactory;
			emailService = _emailService;
			notificationRepository = _notificationRepository;
			httpContextAccessor = _httpContextAccessor;
		}

		public async Task<IdeaResponse> CreateIdea(IdeaRequest idea)
		{
			var academicYearId = httpContextAccessor.HttpContext.User.FindFirstValue("AcademicYearId");
			var departmentId = httpContextAccessor.HttpContext.User.FindFirstValue("DepartmentId");
			var userId = httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			Idea ideaToCreate = new()
			{
				AcademicYearId = academicYearId,
				DepartmentId = departmentId,
				UserId = userId,
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
				CategoryId = result.Entity.CategoryId,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Name = result.Entity.Name,
				Description = result.Entity.Description,
				IsAnonymous = result.Entity.IsAnonymous,
				Author = result.Entity.Name
			};

			var departmentInDb = dbContext.Departments
				.Include(u => u.Users)
				.SingleOrDefault(d => d.Id == departmentId);

			User QAManager = null;

			foreach (var item in departmentInDb.Users)
			{
				var QAManagerInDb = dbContext.UserRoles
					.Include(r => r.Role)
					.Include(u => u.User)
					.SingleOrDefault(u => u.UserId == item.Id && u.Role.Name == "Quality Assurance Manager");

				if(QAManager != null)
				{
					QAManager = QAManagerInDb.User;
				}
			}

			notificationRepository.CreateNotification(QAManager.Id, result.Entity.Id,
				null, "The staff in the department just created a new idea.");

			emailService.SendEmail(QAManager.Email, "Your employee just posted an idea");
			return ideaResponse;
		}

		public async Task<IdeaResponse?> GetIdea(string idIdea)
		{
			var ideaInDb = dbContext.Ideas
				.Include(i => i.Reactions)
				.Include(i => i.Comments)
				.Include(i => i.Documents).SingleOrDefault(i => i.Id == idIdea);

			if (ideaInDb == null)
			{
				return null;
			}
			else
			{
				IdeaResponse ideaResponse = new()
				{
					Id = ideaInDb.Id,
					CategoryId = ideaInDb.CategoryId,
					CreatedAt = ideaInDb.CreatedAt,
					UpdatedAt = ideaInDb.UpdatedAt,
					Name = ideaInDb.Name,
					Description = ideaInDb.Description,
					IsAnonymous = ideaInDb.IsAnonymous,
					Author = ideaInDb.Name,

					Reactions = convertFactory.ConvertListReactions(ideaInDb.Reactions),
					Comments = convertFactory.ConvertListComments(ideaInDb.Comments),
					Documents = convertFactory.ConvertListDocuments(ideaInDb.Documents),
				};
				return ideaResponse;
			}
		}

		public async Task<AllIdeasResponse> GetIdeas(int pageIndex, string? nameIdea)
		{
			var ideasInDb = new List<Idea>();

			if (nameIdea != null)
			{
				ideasInDb = await dbContext.Ideas.Skip((pageIndex - 1) * pageSize).Take(pageSize)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .Include(i => i.Documents).Where(i => i.Name.Contains(nameIdea))
				 .ToListAsync();
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
				Ideas = convertFactory.ConvertListIdeas(ideasInDb)
			};

			return allIdeasResponse;
		}

		public async Task<Idea> RemoveIdea(string idIdea)
		{
			var userId = httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var result = await dbContext.Ideas
				.Include(r => r.Reactions)
				.Include(c => c.Comments)
				.Include(d => d.Documents)
				.SingleOrDefaultAsync(e => e.Id == idIdea && e.UserId == userId);

			if (result != null)
			{
				dbContext.Ideas.Remove(result);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}

		public async Task<IdeaResponse?> UpdateIdea(string idIdea, IdeaRequest idea)
		{
			var userId = httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var ideaInDb = await dbContext.Ideas
				.Include(i => i.Reactions)
				.Include(i => i.Comments)
				.Include(i => i.Documents)
				.SingleOrDefaultAsync(e => e.Id == idIdea && e.UserId == userId);

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
				ideaResponse.CategoryId = ideaInDb.CategoryId;
				ideaResponse.CreatedAt = ideaInDb.CreatedAt;
				ideaResponse.UpdatedAt = ideaInDb.UpdatedAt;
				ideaResponse.Name = ideaInDb.Name;
				ideaResponse.Description = ideaInDb.Description;
				ideaResponse.IsAnonymous = ideaInDb.IsAnonymous;
				ideaResponse.Author = ideaInDb.Name;

				ideaResponse.Reactions = convertFactory.ConvertListReactions(ideaInDb.Reactions);
				ideaResponse.Comments = convertFactory.ConvertListComments(ideaInDb.Comments);
				ideaResponse.Documents = convertFactory.ConvertListDocuments(ideaInDb.Documents);
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

			GetPreSignedUrlRequest request = new()
			{
				BucketName = amazonBucketName,
				Key = fileName + ".jpg",
				Expires = DateTime.Now.AddMinutes(20),
				Verb = HttpVerb.PUT,
			};

			string preSignedUrl = client.GetPreSignedURL(request);

			PreSignedUrlResponse preSignedUrlResponse = new()
			{
				FileName = fileName + ".jpg",
				PreSignedUrl = preSignedUrl,
			};
			return preSignedUrlResponse;
		}

		public async Task<AllIdeasResponse> GetIdeasByUserId(int pageIndex, string? nameIdea)
		{
			var userId = httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var ideasInDb = new List<Idea>();

			if (nameIdea != null)
			{
				ideasInDb = await dbContext.Ideas.Skip((pageIndex - 1) * pageSize).Take(pageSize)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .Include(i => i.Documents)
				 .Where(i => i.Name.Contains(nameIdea) && i.UserId == userId)
				 .ToListAsync();
			}
			else
			{
				ideasInDb = await dbContext.Ideas.Skip((pageIndex - 1) * pageSize).Take(pageSize)
					.Include(i => i.Reactions)
		 			.Include(i => i.Comments)
					.Include(i => i.Documents)
					.Where(i => i.UserId == userId)
					.ToListAsync();
			}

			AllIdeasResponse allIdeasResponse = new()
			{
				PageIndex = pageIndex,
				TotalPage = (int)Math.Ceiling((double)dbContext.Ideas.Count() / pageSize),
				Ideas = convertFactory.ConvertListIdeas(ideasInDb)
			};

			return allIdeasResponse;
		}
	}
}