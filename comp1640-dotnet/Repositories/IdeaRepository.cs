using Amazon.S3.Model;
using Amazon.S3;
using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

using Amazon;

using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.DTOs.Requests;

using comp1640_dotnet.Factory;
using comp1640_dotnet.Services.Interfaces;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class IdeaRepository : IIdeaRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IConfiguration _configuration;
		private static readonly int _pageSize = 5;
		private readonly ConvertFactory _convertFactory;
		private readonly IEmailService _emailService;
		private readonly INotificationRepository _notificationRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public IdeaRepository(ApplicationDbContext dbcontext, 
			IConfiguration configuration,
			ConvertFactory convertFactory,
			IEmailService emailService,
			INotificationRepository notificationRepository,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbcontext;
			_configuration = configuration;
			_convertFactory = convertFactory;
			_emailService = emailService;
			_notificationRepository = notificationRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IdeaResponse?> CreateIdea(IdeaRequest idea)
		{
			var academicYearId = _httpContextAccessor.HttpContext.User.FindFirstValue("AcademicYearId");
			var departmentId = _httpContextAccessor.HttpContext.User.FindFirstValue("DepartmentId");
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			Idea ideaToCreate = new()
			{
				AcademicYearId = academicYearId,
				DepartmentId = departmentId,
				UserId = userId,
				CategoryId = idea.CategoryId,
				Name = idea.Name,
				Description = idea.Description,
				IsLatest = true,
				IsAnonymous = idea.IsAnonymous
			};

			var result = await _dbContext.Ideas.AddAsync(ideaToCreate);
			await _dbContext.SaveChangesAsync();

			if (result == null)
			{
				return null;
			}

			DisableLatestIdeaInDb();

			var author = _dbContext.Profiles.SingleOrDefault(p => p.UserId == userId);

			IdeaResponse ideaResponse = new()
				{
					Id = result.Entity.Id,
					CategoryId = result.Entity.CategoryId,
					CreatedAt = result.Entity.CreatedAt,
					UpdatedAt = result.Entity.UpdatedAt,
					Name = result.Entity.Name,
					Description = result.Entity.Description,
					IsAnonymous = result.Entity.IsAnonymous,
					IsLatest = result.Entity.IsLatest,
					Author = author.FullName
				};

			var departmentInDb = _dbContext.Departments
				.Include(u => u.Users)
				.SingleOrDefault(d => d.Id == departmentId);

			User? QAManager = null;

			foreach (var item in departmentInDb.Users)
				{
					var QAManagerInDb = _dbContext.UserRoles
						.Include(r => r.Role)
						.Include(u => u.User)
						.SingleOrDefault(u => u.UserId == item.Id && u.Role.Name == "Quality Assurance Manager");

					if (QAManager != null)
					{
						QAManager = QAManagerInDb.User;
					}
				}

			_notificationRepository.CreateNotification(QAManager.Id, result.Entity.Id,
					null, "The staff in the department just created a new idea.");

			_emailService.SendEmail(QAManager.Email, "Your employee just posted an idea");
			
			return ideaResponse;
		}

		public async Task<IdeaResponse?> GetIdea(string idIdea)
		{
			var ideaInDb = _dbContext.Ideas
				.Include(i => i.Reactions)
				.Include(i => i.Comments)
				.Include(i => i.Documents).SingleOrDefault(i => i.Id == idIdea);

			var author = _dbContext.Profiles.SingleOrDefault(a => a.UserId == ideaInDb.UserId);

			if (ideaInDb == null)
			{
				return null;
			}

			ideaInDb.ViewCount += 1;
			await _dbContext.SaveChangesAsync();

			IdeaResponse ideaResponse = new()
				{
					Id = ideaInDb.Id,
					CategoryId = ideaInDb.CategoryId,
					CreatedAt = ideaInDb.CreatedAt,
					UpdatedAt = ideaInDb.UpdatedAt,
					Name = ideaInDb.Name,
					Description = ideaInDb.Description,
					IsAnonymous = ideaInDb.IsAnonymous,
					IsLatest = ideaInDb.IsLatest,
					Author = author.FullName,

					Reactions = _convertFactory.ConvertListReactions(ideaInDb.Reactions),
					Comments = _convertFactory.ConvertListComments(ideaInDb.Comments),
					Documents = _convertFactory.ConvertListDocuments(ideaInDb.Documents),
				};
			return ideaResponse;
		}

		public async Task<AllIdeasResponse> GetIdeas(int pageIndex, string? nameIdea)
		{
			var ideasInDb = new List<Idea>();

			if (nameIdea != null)
			{
				ideasInDb = await _dbContext.Ideas.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .Include(i => i.Documents).Where(i => i.Name.Contains(nameIdea))
				 .ToListAsync();
			}
			else
			{
				ideasInDb = await _dbContext.Ideas.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
					.Include(i => i.Reactions)
		 			.Include(i => i.Comments)
					.Include(i => i.Documents).ToListAsync();
			}

			AllIdeasResponse allIdeasResponse = new()
			{
				PageIndex = pageIndex,
				TotalPage = (int)Math.Ceiling((double)_dbContext.Ideas.Count() / _pageSize),
				Ideas = _convertFactory.ConvertListIdeas(ideasInDb)
			};

			return allIdeasResponse;
		}

		public async Task<Idea> RemoveIdea(string idIdea)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var result = await _dbContext.Ideas
				.Include(r => r.Reactions)
				.Include(c => c.Comments)
				.Include(d => d.Documents)
				.SingleOrDefaultAsync(e => e.Id == idIdea && e.UserId == userId);

			if (result != null)
			{
				_dbContext.Ideas.Remove(result);
				await _dbContext.SaveChangesAsync();
			}
			return result;
		}

		public async Task<IdeaResponse?> UpdateIdea(string idIdea, IdeaRequest idea)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var ideaInDb = await _dbContext.Ideas
				.Include(i => i.Reactions)
				.Include(i => i.Comments)
				.Include(i => i.Documents)
				.SingleOrDefaultAsync(e => e.Id == idIdea && e.UserId == userId);


			if (ideaInDb == null)
			{
				return null;
			}

			IdeaResponse? ideaResponse = new();

			ideaInDb.Name = idea.Name;
			ideaInDb.Description = idea.Description;
			ideaInDb.IsAnonymous = idea.IsAnonymous;
			await _dbContext.SaveChangesAsync();

			ideaResponse.Id = ideaInDb.Id;
			ideaResponse.CategoryId = ideaInDb.CategoryId;
			ideaResponse.CreatedAt = ideaInDb.CreatedAt;
			ideaResponse.UpdatedAt = ideaInDb.UpdatedAt;
			ideaResponse.Name = ideaInDb.Name;
			ideaResponse.Description = ideaInDb.Description;
			ideaResponse.IsAnonymous = ideaInDb.IsAnonymous;
			ideaResponse.IsLatest = ideaInDb.IsLatest;
			ideaResponse.Author = ideaInDb.Name;

			ideaResponse.Reactions = _convertFactory.ConvertListReactions(ideaInDb.Reactions);
			ideaResponse.Comments = _convertFactory.ConvertListComments(ideaInDb.Comments);
			ideaResponse.Documents = _convertFactory.ConvertListDocuments(ideaInDb.Documents);
			
			return ideaResponse;
		}

		public PreSignedUrlResponse GetS3PreSignedUrl()
		{
			var amazonAccessKey = _configuration["AWS:ACCESS_KEY"];
			var amazonSecretKey = _configuration["AWS:SECRET_KEY"];
			var amazonBucketName = _configuration["AWS:BUCKET_NAME"];
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
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var ideasInDb = new List<Idea>();

			if (nameIdea != null)
			{
				ideasInDb = await _dbContext.Ideas.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .Include(i => i.Documents)
				 .Where(i => i.Name.Contains(nameIdea) && i.UserId == userId)
				 .ToListAsync();
			}
			else
			{
				ideasInDb = await _dbContext.Ideas.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
					.Include(i => i.Reactions)
		 			.Include(i => i.Comments)
					.Include(i => i.Documents)
					.Where(i => i.UserId == userId)
					.ToListAsync();
			}

			AllIdeasResponse allIdeasResponse = new()
			{
				PageIndex = pageIndex,
				TotalPage = (int)Math.Ceiling((double)_dbContext.Ideas.Count() / _pageSize),
				Ideas = _convertFactory.ConvertListIdeas(ideasInDb)
			};

			return allIdeasResponse;
		}

		public async Task<Idea?> IdeaExistsInDb(string idIdea)
		{
			var ideaInDb =  _dbContext.Ideas
				.Include(u => u.User)
				.Include(c => c.Comments)
				.SingleOrDefault(i => i.Id == idIdea);

			if(ideaInDb == null)
			{
				return null; 
			}
			return ideaInDb;
		}

		private async void DisableLatestIdeaInDb()
		{
			var latestIdea = _dbContext.Ideas
				.OrderByDescending(i => i.CreatedAt)
				.Where(c => c.IsLatest == true)
				.ElementAtOrDefault(1);

			if (latestIdea != null)
			{
				latestIdea.IsLatest = false;
				await _dbContext.SaveChangesAsync();
			}
		}
	}
}