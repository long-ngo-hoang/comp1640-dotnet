using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetCategories();
		Task<Category> GetCategory(string idCategory);
		Task<Category> CreateCategory(Category category);
		Task<Category> RemoveCategory (string idCategory);
		Task<Category> UpdateCategory(string idCategory, Category category);
	}
}
