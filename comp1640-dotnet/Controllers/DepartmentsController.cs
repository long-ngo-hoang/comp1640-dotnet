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
	public class DepartmentsController : ControllerBase
	{
		private readonly IDepartmentRepository _departmentRepos;

		public DepartmentsController(IDepartmentRepository departmentRepos)
		{
			_departmentRepos = departmentRepos;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
		{
			var result = await _departmentRepos.GetDepartments();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<DepartmentResponse>> GetDepartment(string id, int pageIndex = 1 )
		{
			var result = await _departmentRepos.GetDepartment(id, pageIndex);
			if(result == null)
			{
				return BadRequest("Department not found");
			}
			return Ok(result);
		}

		[HttpGet("GetDepartmentByQACoordinator")]
		public async Task<ActionResult<DepartmentResponse>> GetDepartmentByQACoordinator()
		{
			var result = await _departmentRepos.GetDepartmentByQACoordinator();
			if (result == null)
			{
				return BadRequest("Department not found");
			}
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<DepartmentResponse>> CreateDepartment(DepartmentRequest department)
		{
			var result = await _departmentRepos.CreateDepartment(department);
			return Ok(result);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<DepartmentResponse>> UpdateDepartment(string id, DepartmentRequest department)
		{
			var result = await _departmentRepos.UpdateDepartment(id, department);
			if (result == null)
			{
				return BadRequest("Department not found");
			}
			return Ok("Update successful department");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Department>> RemoveDocument(string id)
		{
			var result = await _departmentRepos.RemoveDepartment(id);
			if (result == null)
			{
				return BadRequest("Department not found");
			}
			return Ok("Delete successful Department");
		}

		[HttpPut("AddUserToDepartment/{userId}")]
		public async Task<ActionResult<DepartmentResponse>> AddUserToDepartment(string userId, string departmentId)
		{
			var result = await _departmentRepos.AddUserToDepartment(userId, departmentId);

			if(result == null)
			{
				return BadRequest("User not found");
			}
			return Ok(result);
		}

		[HttpPut("RemoveUserFromDepartment/{userId}")]
		public async Task<ActionResult<DepartmentResponse>> RemoveUserFromDepartment(string userId)
		{
			var result = await _departmentRepos.RemoveUserFromDepartment(userId);

			if (result == null)
			{
				return BadRequest("User not found");
			}
			return Ok(result);
		}

		[HttpGet("GetStatisticalAnalysis")]
		public async Task<ActionResult<IEnumerable<Department>>> GetStatisticalAnalysis()
		{
			var result = await _departmentRepos.GetStatisticalAnalysis();

			if (result == null)
			{
				return BadRequest("");
			}
			return Ok(result);
		}
	}
}
