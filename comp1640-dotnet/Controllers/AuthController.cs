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
		public async Task<ActionResult<User>> Register(UserRegisterRequest userRegisterRequest)
		{
			var hmac = new HMACSHA512();

			user.UserName = userRegisterRequest.Email;
			user.Email = userRegisterRequest.Email;
			user.DepartmentId = userRegisterRequest.DepartmentId;

			user.PasswordSalt = PasswordSalt(hmac);
			user.PasswordHash = PasswordHash(userRegisterRequest.Password, hmac);

			var result = dbContext.Users.Add(user);
			await dbContext.SaveChangesAsync();

			return Ok(user);
		}

		private static byte[] PasswordSalt(HMACSHA512 hmac)
		{
			return hmac.Key;
		}

		private static byte[] PasswordHash(string password, HMACSHA512 hmac)
		{
			return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		[HttpPost("Login")]
		public async Task<ActionResult<User>> Login(UserLoginRequest userLoginRequest)
		{
			var user = dbContext.Users.FirstOrDefault(u => u.Email == userLoginRequest.Email);
			if (user == null)
			{
				return BadRequest("Account Not Found.");
			}
			if (!VerifyPassword(userLoginRequest.Password, user.PasswordHash, user.PasswordSalt))
			{
				return BadRequest("Wrong Password");
			}
			return Ok("Logged in successfully");
		}

		private static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			var hmac = new HMACSHA512(passwordSalt);
			var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(passwordHash);
		}
	}
}
