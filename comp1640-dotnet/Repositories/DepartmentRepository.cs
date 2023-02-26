using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Factory;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace comp1640_dotnet.Repositories
{
	public class DepartmentRepository : IDepartmentRepository
	{
		private readonly ApplicationDbContext dbContext;
		private readonly ConvertFactory convertFactory;
		private static readonly int pageSize = 5;


		public DepartmentRepository(ApplicationDbContext context, ConvertFactory _convertFactory)
		{
			dbContext = context;
			convertFactory = _convertFactory;
		}

		public async Task<DepartmentResponse> CreateDepartment(DepartmentRequest department)
		{
			Department departmentToCreate = new()
			{
				Name = department.Name 
			};

			var result = await dbContext.Departments.AddAsync(departmentToCreate);
			await dbContext.SaveChangesAsync();

			DepartmentResponse departmentResponse = new()
			{
				Id = result.Entity.Id,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Name = result.Entity.Name
			};

			return departmentResponse;
		}

		public async Task<DepartmentResponse> GetDepartment(string idDepartment, int pageIndex)
		{
			var departmentInDB = dbContext.Departments
				.Include(i => i.Ideas
					.Skip((pageIndex - 1) * pageSize)
					.Take(pageSize))
				.SingleOrDefault(i => i.Id == idDepartment);

			DepartmentResponse departmentResponse = new()
			{
				Id = departmentInDB.Id,
				CreatedAt = departmentInDB.CreatedAt,
				UpdatedAt = departmentInDB.UpdatedAt,
				Name = departmentInDB.Name,
				AllIdeas = new AllIdeasResponse()
				{
					PageIndex = pageIndex,
					TotalPage = (int)Math.Ceiling((double)dbContext.Ideas.Count() / pageSize),
					Ideas = convertFactory.ConvertListIdeas(departmentInDB.Ideas)
				}
			};
			return departmentResponse;
		}

		public async Task<IEnumerable<Department>> GetDepartments()
		{
			return await dbContext.Departments.ToListAsync();
		}
	}
}
