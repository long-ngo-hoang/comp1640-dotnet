using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

		[HttpGet("{userId}")]
		public async Task<ActionResult<UserRole>> GetUserRoles(string userId)
		{
			var result = await _userRoleRepos.GetUserRole(userId);

			if (result == null)
			{
				return BadRequest("User Roles not found");
			}
			return Ok(result);
		}

		[HttpPut("{userId}")]
		public async Task<ActionResult<UserRole>> UpdateUserRoles(string userId, string roleId)
		{
			var result = await _userRoleRepos.UpdateUserRole(userId, roleId);

			if(result == null)
			{
				return BadRequest("User not found");
			}
			return Ok(result);
		}
	}
}
