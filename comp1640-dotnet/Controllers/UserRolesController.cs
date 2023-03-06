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
	[Authorize(Roles = "Administrator")]
	public class UserRolesController : ControllerBase
	{
		private readonly IUserRoleRepository _userRoleRepos;

		public UserRolesController(IUserRoleRepository userRoleRepos)
		{
			_userRoleRepos = userRoleRepos;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserRole>> GetUserRoles(string id)
		{
			var result = await _userRoleRepos.GetUserRole(id);

			if (result == null)
			{
				return BadRequest("User Roles not found");
			}
			return Ok(result);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<UserRole>> UpdateUserRoles(string id, string roleId)
		{
			var result = await _userRoleRepos.UpdateUserRole(id, roleId);

			if(result == null)
			{
				return BadRequest("User not found");
			}
			return Ok(result);
		}
	}
}
