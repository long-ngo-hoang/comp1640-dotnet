using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryRepository categoryRepos;

		public CategoriesController(ICategoryRepository _categoryRepos)
		{
			this.categoryRepos = _categoryRepos;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
		{
			var result = await categoryRepos.GetCategories();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryResponse>> GetCategory(string id)
		{
			var result = await categoryRepos.GetCategory(id);
			if(result == null)
			{
				return BadRequest("Category not found");
			}
			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "Quality Assurance Manager")]
		public async Task<ActionResult<CategoryResponse>> CreateCategory(CategoryRequest category)
		{
			var result = await categoryRepos.CreateCategory(category);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Quality Assurance Manager")]
		public async Task<ActionResult<Category>> RemoveCategory(string id)
		{
			var result = await categoryRepos.RemoveCategory(id);

			if(result == null)
			{
				return BadRequest("Category not found");
			}

			else if(result.Ideas.Count() == 0)
			{
				return Ok("Delete successful category");
			}
				return BadRequest("the category cannot be deleted because of the idea of using it.");
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Quality Assurance Manager")]
		public async Task<ActionResult<CategoryResponse>> UpdateCategory(string id, CategoryRequest category)
		{
			var result = await categoryRepos.UpdateCategory(id, category);
			if (result == null)
			{
				return BadRequest("Category not found");
			}
			return Ok(result);
		}
	}
}
