using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public RoleRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Role>?> GetRoles()
		{
			var rolesInDb = _dbContext.Roles.Where(r => r.Name != "Administrator");

			if(rolesInDb == null)
			{
				return null;
			}

			return rolesInDb;
		}
	}
}
