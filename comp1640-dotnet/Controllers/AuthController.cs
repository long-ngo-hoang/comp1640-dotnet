using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace comp1640_dotnet.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ApplicationDbContext? dbContext;
		public AuthController(ApplicationDbContext? dbContext)
		{
			this.dbContext = dbContext;
		}

		public static User user = new User();

		[HttpPost("Register")]
		public async Task<ActionResult<User>> Register(UserRequest userRequest)
		{
			user.UserName = userRequest.Email;
			user.Email = userRequest.Email;
			user.DepartmentId = userRequest.DepartmentId;	
			user.Password =  PasswordHash(userRequest.Password);
			var result = dbContext.Users.Add(user);
			await dbContext.SaveChangesAsync();

			return Ok(user);
		}

		private static byte[] PasswordHash(string password)
		{
			var hmac = new HMACSHA512();
			return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}
	}
}
