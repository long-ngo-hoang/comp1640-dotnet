﻿using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Factory;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class DepartmentRepository : IDepartmentRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly ConvertFactory _convertFactory;
		private static readonly int _pageSize = 5;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public DepartmentRepository(ApplicationDbContext dbContext, ConvertFactory convertFactory, IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_convertFactory = convertFactory;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<DepartmentResponse> CreateDepartment(DepartmentRequest department)
		{
			Department departmentToCreate = new()
			{
				Name = department.Name 
			};

			var result = await _dbContext.Departments.AddAsync(departmentToCreate);
			await _dbContext.SaveChangesAsync();

			DepartmentResponse departmentResponse = new()
			{
				Id = result.Entity.Id,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Name = result.Entity.Name
			};

			return departmentResponse;
		}

		public async Task<DepartmentResponse?> GetDepartment(string idDepartment)
		{
			var departmentInDB = _dbContext.Departments
				.Include(i => i.Ideas)
				.ThenInclude(u => u.User)
				.Include(u => u.Users)
				.SingleOrDefault(i => i.Id == idDepartment);
			
			if(departmentInDB == null)
			{
				return null;
			}

			DepartmentResponse departmentResponse = new()
			{
				Id = departmentInDB.Id,
				CreatedAt = departmentInDB.CreatedAt,
				UpdatedAt = departmentInDB.UpdatedAt,
				Name = departmentInDB.Name,
				AllIdeas = new AllIdeasResponse()
				{
					Ideas = _convertFactory.ConvertListIdeas(departmentInDB.Ideas)
				},
				AllUsers = _convertFactory.ConvertListUsers(departmentInDB.Users)
			};
			return departmentResponse;
		}

		public async Task<DepartmentResponse?> GetDepartmentByQACoordinator()
		{
			var departmentId = _httpContextAccessor.HttpContext.User.FindFirstValue("DepartmentId");

			var departmentInDB = _dbContext.Departments
				.Include(i => i.Ideas)
				.ThenInclude(u => u.User)
				.Include(u => u.Users)

				.SingleOrDefault(i => i.Id == departmentId);

			if (departmentInDB == null)
			{
				return null;
			}

			DepartmentResponse departmentResponse = new()
			{
				Id = departmentInDB.Id,
				CreatedAt = departmentInDB.CreatedAt,
				UpdatedAt = departmentInDB.UpdatedAt,
				Name = departmentInDB.Name,
				AllIdeas = new AllIdeasResponse()
				{
					Ideas = _convertFactory.ConvertListIdeas(departmentInDB.Ideas)
				},
				AllUsers = _convertFactory.ConvertListUsers(departmentInDB.Users)

			};
			return departmentResponse;
		}


		public async Task<IEnumerable<Department>> GetDepartments()
		{
			return await _dbContext.Departments.ToListAsync();
		}

		public async Task<DepartmentResponse?> UpdateDepartment(string idDepartment, DepartmentRequest department)
		{
			var departmentInDb = await _dbContext.Departments
							 .SingleOrDefaultAsync(e => e.Id == idDepartment);

			DepartmentResponse departmentResponse = new();

			if (departmentInDb == null)
			{
				return null;
			}

			departmentInDb.Name = department.Name;
			await _dbContext.SaveChangesAsync();

			departmentResponse.Id = departmentInDb.Id;
			departmentResponse.CreatedAt = departmentInDb.CreatedAt;
			departmentResponse.UpdatedAt = departmentInDb.UpdatedAt;
			departmentResponse.Name = departmentInDb.Name;

			return departmentResponse;
		}

		public async Task<Department?> RemoveDepartment(string idDepartment)
		{
			var result = await _dbContext.Departments
							 .SingleOrDefaultAsync(e => e.Id == idDepartment);

			if (result == null)
			{
				return null;
			}

			_dbContext.Departments.Remove(result);
			await _dbContext.SaveChangesAsync();

			return result;
		}

		public async Task<DepartmentResponse?> RemoveUserFromDepartment(string userId)
		{
			var userInDb = _dbContext.Users
				.Include(d => d.Department)
				.SingleOrDefault(u => u.Id == userId);

			if(userInDb == null)
			{
				return null;
			}
			var departmentInDb = userInDb.Department;

			userInDb.DepartmentId = null;
			await _dbContext.SaveChangesAsync();

			DepartmentResponse departmentResponse = new()
			{
				Id = departmentInDb.Id,
				CreatedAt = departmentInDb.CreatedAt,
				UpdatedAt = departmentInDb.UpdatedAt,
				Name = departmentInDb.Name
			};

			return departmentResponse;
		}

		public async Task<DepartmentResponse?> AddUserToDepartment(string userId, string departmentId)
		{
			var userInDb = _dbContext.Users
				.SingleOrDefault(u => u.Id == userId);

			var departmentInDb = _dbContext.Departments
				.SingleOrDefault(u => u.Id == departmentId);

			if (userInDb == null)
			{
				return null;
			}

			userInDb.DepartmentId = departmentId;

			await _dbContext.SaveChangesAsync();

			DepartmentResponse departmentResponse = new()
			{
				Id = departmentInDb.Id,
				CreatedAt = departmentInDb.CreatedAt,
				UpdatedAt = departmentInDb.UpdatedAt,
				Name = departmentInDb.Name
			};

			return departmentResponse;
		}

		public async Task<IEnumerable<DepartmentResponse>> GetStatisticalAnalysis()
		{
			var departmentsInDb = _dbContext.Departments
				.Include(i => i.Ideas)
				.ToList();

			var departments = departmentsInDb.Select(x => new DepartmentResponse()
			{
				Id = x.Id,
				CreatedAt = x.CreatedAt,
				UpdatedAt = x.UpdatedAt,
				Name = x.Name,
				TotalIdeas = x.Ideas.Count(),
			}).ToList();

			return departments;

		}
	}
}
