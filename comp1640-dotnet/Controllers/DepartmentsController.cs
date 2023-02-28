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
	public class DepartmentsController : ControllerBase
	{
		private readonly IDepartmentRepository departmentRepos;

		public DepartmentsController(IDepartmentRepository _departmentRepos)
		{
			this.departmentRepos = _departmentRepos;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
		{
			var result = await departmentRepos.GetDepartments();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<DepartmentResponse>> GetDepartment(string id, int pageIndex = 1 )
		{
			var result = await departmentRepos.GetDepartment(id, pageIndex);
			if(result == null)
			{
				return BadRequest("Department not found");
			}
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<DepartmentResponse>> CreateDepartment(DepartmentRequest department)
		{
			var result = await departmentRepos.CreateDepartment(department);
			return Ok(result);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<DepartmentResponse>> UpdateDepartment(string id, DepartmentRequest department)
		{
			var result = await departmentRepos.UpdateDepartment(id, department);
			if (result == null)
			{
				return BadRequest("Department not found");
			}
			return Ok("Update successful department");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Department>> RemoveDocument(string id)
		{
			var result = await departmentRepos.RemoveDepartment(id);
			if (result == null)
			{
				return BadRequest("Department not found");
			}
			return Ok("Delete successful Department");
		}
	}
}
