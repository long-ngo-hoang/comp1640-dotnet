using comp1640_dotnet.Data;
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
		public async Task<ActionResult<Category>> GetCategory(string id)
		{
			var result = await categoryRepos.GetCategory(id);
			if(result == null)
			{
				return BadRequest("Category not found");
			}
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Category>> CreateCategory(Category category)
		{
			var result = await categoryRepos.CreateCategory(category);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Category>> RemoveCategory(string id)
		{
			var result = await categoryRepos.RemoveCategory(id);
			if(result == null)
			{
				return BadRequest("Category not found");
			}
			return Ok("Delete successful category");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Category>> UpdateCategory(string id, Category category)
		{
			var result = await categoryRepos.UpdateCategory(id, category);
			if (result == null)
			{
				return BadRequest("Category not found");
			}
			return Ok("Update successful category");
		}
	}
}
