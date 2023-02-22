using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetCategories();
		Task<CategoryResponse> GetCategory(string idCategory);
		Task<CategoryResponse> CreateCategory(CategoryRequest category);
		Task<Category> RemoveCategory (string idCategory);
		Task<CategoryResponse> UpdateCategory(string idCategory, CategoryRequest category);
	}
}
