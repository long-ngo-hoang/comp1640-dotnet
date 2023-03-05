using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public CategoryRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<CategoryResponse> CreateCategory(CategoryRequest category)
		{
			Category categoryToCreate = new() {
				Name = category.Name 
			};

			var result = await _dbContext.Categories.AddAsync(categoryToCreate);
			await _dbContext.SaveChangesAsync();

			CategoryResponse categoryResponse = new()
			{
				Id = result.Entity.Id,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				Name = result.Entity.Name
			};

			return categoryResponse;
		}

		public async Task<CategoryResponse?> GetCategory(string idCategory)
		{
			var categoryInDB = _dbContext.Categories.SingleOrDefault(i => i.Id == idCategory);

			if(categoryInDB == null)
			{
				return null;
			}
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
			return await _dbContext.Categories.ToListAsync();
		}

		public async Task<Category?> RemoveCategory(string idCategory)
		{
			var result = await _dbContext.Categories.Include(i => i.Ideas)
							 .SingleOrDefaultAsync(e => e.Id == idCategory);

			if(result == null)
			{
				return null;
			}
			else if(result.Ideas.Count() != 0)
			{
				return result;
			}
			else if (result != null && result.Ideas.Count() == 0)
			{
				_dbContext.Categories.Remove(result);
				await _dbContext.SaveChangesAsync();
			}
			return result;
		}

		public async Task<CategoryResponse?> UpdateCategory(string idCategory, CategoryRequest category)
		{
			CategoryResponse? categoryResponse = new();

			var categoryInDb = await _dbContext.Categories
							 .SingleOrDefaultAsync(e => e.Id == idCategory);

			if (categoryInDb == null)
			{
				return null;
			}

			categoryInDb.Name = category.Name;
			await _dbContext.SaveChangesAsync();

			categoryResponse.Id = categoryInDb.Id;
			categoryResponse.CreatedAt = categoryInDb.CreatedAt;
			categoryResponse.UpdatedAt = categoryInDb.UpdatedAt;
			categoryResponse.Name = categoryInDb.Name;

			return categoryResponse;
		}
	}
}
