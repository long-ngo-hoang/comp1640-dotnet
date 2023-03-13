using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserRepository(ApplicationDbContext dbContext,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<User?> GetUser(string userId)
		{
			var userInDb = await _dbContext.Users
				.SingleOrDefaultAsync(d => d.Id == userId);

			if (userInDb == null)
			{
				return null;
			}
			return userInDb;
		}

		public async Task<User?> GetAuthor(string ideaId)
		{
			var ideaInDb = _dbContext.Ideas
				.Include(u => u.User)
				.SingleOrDefault(d => d.Id == ideaId);

			if(ideaInDb == null)
			{
				return null;
			}
			User author = ideaInDb.User;

			return author;
		}

		public async Task<User?> GetQACoordinator()
		{
			var departmentId = _httpContextAccessor.HttpContext.User.FindFirstValue("DepartmentId");

			var departmentInDb = _dbContext.Departments
				.Include(u => u.Users)
				.SingleOrDefault(d => d.Id == departmentId);

			User QAManager = null;

			foreach (var item in departmentInDb.Users)
			{
				var QAManagerInDb = _dbContext.UserRoles
					.Include(r => r.Role)
					.Include(u => u.User)
					.SingleOrDefault(u => u.UserId == item.Id && u.Role.Name == "Quality Assurance Coordinator");

				if (QAManagerInDb != null)
				{
					QAManager = QAManagerInDb.User;
				}
			}
			return QAManager;
		}
	}
}
