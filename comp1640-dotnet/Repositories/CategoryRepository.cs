using comp1640_dotnet.Data;
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

		public async Task<Category> CreateCategory(Category category)
{
			var result = await dbContext.Categories.AddAsync(category);
			await dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Category> GetCategory(string idCategory)
		{
			return dbContext.Categories.SingleOrDefault(i => i.Id == idCategory);
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

		public async Task<Category> UpdateCategory(string idCategory, Category category)
		{
			var categoryInDb = await dbContext.Categories
							 .SingleOrDefaultAsync(e => e.Id == idCategory);

			if (categoryInDb != null)
			{
				categoryInDb.Name = category.Name;
				await dbContext.SaveChangesAsync();
			}
			return categoryInDb;
		}
	}
}
