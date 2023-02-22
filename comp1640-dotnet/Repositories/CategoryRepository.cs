using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace comp1640_dotnet.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext dbContext;

		public CategoryRepository(ApplicationDbContext context)
		{
			dbContext = context;
		}

		public async Task<CategoryResponse> CreateCategory(CategoryRequest category)
		{
			Category categoryToCreate = new() {
				Name = category.Name 
			};

			var result = await dbContext.Categories.AddAsync(categoryToCreate);
			await dbContext.SaveChangesAsync();

			CategoryResponse categoryResponse = new()
			{
				Id = result.Entity.Id,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Name = result.Entity.Name
			};

			return categoryResponse;
		}

		public async Task<CategoryResponse> GetCategory(string idCategory)
		{
			var categoryInDB = dbContext.Categories.SingleOrDefault(i => i.Id == idCategory);

			CategoryResponse categoryResponse = new()
			{
				Id = categoryInDB.Id,
				CreatedAt = categoryInDB.CreatedAt,
				UpdatedAt = categoryInDB.UpdatedAt,
				Name = categoryInDB.Name
			};
			return categoryResponse;
		}

		public async Task<IEnumerable<Category>> GetCategories()
		{
			return await dbContext.Categories.ToListAsync();
		}

		public async Task<Category> RemoveCategory(string idCategory)
		{
			var result = await dbContext.Categories
							 .SingleOrDefaultAsync(e => e.Id == idCategory);

			if (result != null)
			{
				dbContext.Categories.Remove(result);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}

		public async Task<CategoryResponse?> UpdateCategory(string idCategory, CategoryRequest category)
		{
			CategoryResponse? categoryResponse = new();

			var categoryInDb = await dbContext.Categories
							 .SingleOrDefaultAsync(e => e.Id == idCategory);

			if (categoryInDb == null)
			{
				return null;
			}
			else
			{
				categoryInDb.Name = category.Name;
				await dbContext.SaveChangesAsync();
				categoryResponse.Id = categoryInDb.Id;
				categoryResponse.CreatedAt = categoryInDb.CreatedAt;
				categoryResponse.UpdatedAt = categoryInDb.UpdatedAt;
				categoryResponse.Name = categoryInDb.Name;
				return categoryResponse;
			}
		}
	}
}
