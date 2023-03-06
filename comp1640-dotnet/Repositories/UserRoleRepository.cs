using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Repositories
{
	public class UserRoleRepository : IUserRoleRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public UserRoleRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<UserRole?> GetUserRole(string userId)
		{
			var userRoleInDb = _dbContext.UserRoles
				.Include(u => u.User)
				.Include(r => r.Role)
				.SingleOrDefault(u => u.UserId == userId);

			if (userRoleInDb == null)
			{
				return null;
			}

			return userRoleInDb;
		}

		public async Task<UserRole?> UpdateUserRole(string userId, string roleId)
		{
			var userRoleInDb = _dbContext.UserRoles.SingleOrDefault(u => u.UserId == userId);

			if(userRoleInDb == null)
			{
				return null;
			}

			userRoleInDb.RoleId = roleId;
			await _dbContext.SaveChangesAsync();

			return userRoleInDb;
		}
	}
}
