using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.Factory;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class IdeaRepository : IIdeaRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IConfiguration _configuration;
		private static readonly int _pageSize = 5;
		private readonly ConvertFactory _convertFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public IdeaRepository(ApplicationDbContext dbcontext, 
			IConfiguration configuration,
			ConvertFactory convertFactory,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbcontext;
			_configuration = configuration;
			_convertFactory = convertFactory;
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
				IsAnonymous = idea.IsAnonymous
			};

			var result = await _dbContext.Ideas.AddAsync(ideaToCreate);
			await _dbContext.SaveChangesAsync();

			if (result == null)
			{
				return null;
			}

			var author = _dbContext.Users.SingleOrDefault(p => p.Id == userId);

			IdeaResponse ideaResponse = new()
				{
					Id = result.Entity.Id,
					CategoryId = result.Entity.CategoryId,
					CreatedAt = result.Entity.CreatedAt,
					UpdatedAt = result.Entity.UpdatedAt,
					Name = result.Entity.Name,
					Description = result.Entity.Description,
					IsAnonymous = result.Entity.IsAnonymous,
					ViewCount = result.Entity.ViewCount,
					Author = author.UserName
				};
			
			return ideaResponse;
		}

		public async Task<IdeaResponse?> GetIdea(string idIdea)
		{
			var ideaInDb = _dbContext.Ideas
				.Include(u => u.User)
				.Include(i => i.Reactions)
				.Include(i => i.Comments.OrderByDescending(c => c.CreatedAt))
				.ThenInclude(u => u.User)
				.Include(i => i.Documents).SingleOrDefault(i => i.Id == idIdea);

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
					ViewCount = ideaInDb.ViewCount,
					Author = ideaInDb.User.UserName,

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
				ideasInDb = await _dbContext.Ideas
				 .Include(u => u.User)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .ThenInclude(u => u.User)
				 .Include(i => i.Documents).Where(i => i.Name.Contains(nameIdea))
				 .OrderByDescending(i => i.CreatedAt)
				 .Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
				 .ToListAsync();
			}
			else
			{
				ideasInDb = await _dbContext.Ideas
					.Include(u => u.User)
					.Include(i => i.Reactions)
		 			.Include(i => i.Comments)
					.ThenInclude(u => u.User)
					.Include(i => i.Documents)
					.OrderByDescending(i => i.CreatedAt)
				  .Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
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

		public async Task<Idea> RemoveIdea(string idIdea)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var result = await _dbContext.Ideas
				.Include(n => n.Notification)
				.Include(r => r.Reactions)
				.Include(c => c.Comments)
				.ThenInclude(n => n.Notification)
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
			ideaResponse.ViewCount = ideaInDb.ViewCount;
			ideaResponse.Author = ideaInDb.Name;

			ideaResponse.Reactions = _convertFactory.ConvertListReactions(ideaInDb.Reactions);
			ideaResponse.Comments = _convertFactory.ConvertListComments(ideaInDb.Comments);
			ideaResponse.Documents = _convertFactory.ConvertListDocuments(ideaInDb.Documents);
			
			return ideaResponse;
		}

		public async Task<AllIdeasResponse?> GetIdeasByUserId(int pageIndex, string? nameIdea)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var ideasInDb = new List<Idea>();

			if (nameIdea != null)
			{
				ideasInDb = await _dbContext.Ideas
				 .Include(u => u.User)
				 .Include(i => i.Reactions)
				 .Include(i => i.Comments)
				 .ThenInclude(u => u.User)
				 .Include(i => i.Documents)
				 .Where(i => i.Name.Contains(nameIdea) && i.UserId == userId)
				 .OrderByDescending(i => i.CreatedAt)
				 .Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
				 .ToListAsync();
			}
			else
			{
				ideasInDb = await _dbContext.Ideas
					.Include(u => u.User)
					.Include(i => i.Reactions)
		 			.Include(i => i.Comments)
					.ThenInclude(u => u.User)
					.Include(i => i.Documents)
					.Where(i => i.UserId == userId)
				  .OrderByDescending(i => i.CreatedAt)
				  .Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
					.ToListAsync();
			}

			if (ideasInDb == null)
			{	
				return null;
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

		public async Task<AllIdeasResponse> GetMostPopularIdeas(int pageIndex)
		{
			var ideasInDb = new List<Idea>();

			ideasInDb = await _dbContext.Ideas
				.Include(u => u.User)
				.Include(i => i.Reactions)
	 			.Include(i => i.Comments)
				.ThenInclude(u => u.User)
				.Include(i => i.Documents)
				.OrderByDescending(i => i.Reactions.Count)
				.Skip((pageIndex - 1) * _pageSize)
				.Take(_pageSize).ToListAsync();

			AllIdeasResponse allIdeasResponse = new()
			{
				PageIndex = pageIndex,
				TotalPage = (int)Math.Ceiling((double)_dbContext.Ideas.Count() / _pageSize),
				Ideas = _convertFactory.ConvertListIdeas(ideasInDb)
			};

			return allIdeasResponse;
		}

		public async Task<AllIdeasResponse> GetMostViewedIdeas(int pageIndex)
		{
			var ideasInDb = new List<Idea>();

			ideasInDb = await _dbContext.Ideas
				.Include(u => u.User)
				.Include(i => i.Reactions)
	 			.Include(i => i.Comments)
				.ThenInclude(u => u.User)
				.Include(i => i.Documents)
				.OrderByDescending(i => i.ViewCount)
				.Skip((pageIndex - 1) * _pageSize)
				.Take(_pageSize).ToListAsync();

			AllIdeasResponse allIdeasResponse = new()
			{
				PageIndex = pageIndex,
				TotalPage = (int)Math.Ceiling((double)_dbContext.Ideas.Count() / _pageSize),
				Ideas = _convertFactory.ConvertListIdeas(ideasInDb)
			};

			return allIdeasResponse;
		}
	}
}