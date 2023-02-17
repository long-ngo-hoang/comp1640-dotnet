using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace comp1640_dotnet.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ApplicationDbContext? dbContext;
		private readonly IConfiguration configuration;

		public AuthController(ApplicationDbContext? dbContext, IConfiguration configuration)
		{
			this.dbContext = dbContext;
			this.configuration = configuration;
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
			var user = dbContext.Users.Include(t => t.RefreshToken)
				.FirstOrDefault(u => u.Email == userLoginRequest.Email);

			if (user == null)
			{
				return BadRequest("Account Not Found.");
			}
			if (!VerifyPassword(userLoginRequest.Password, user.PasswordHash, user.PasswordSalt))
			{
				return BadRequest("Wrong Password");
			}

			string token = CreateToken(user);

			var refreshToken = GenerateRefreshToken();

			if(user.RefreshToken == null)
			{
				dbContext.Add(refreshToken);
			}
			else
			{
				user.RefreshToken = refreshToken;

			}
			await dbContext.SaveChangesAsync();

			SetRefreshToken(refreshToken);

			return Ok(token);
		}

		private static RefreshToken GenerateRefreshToken()
		{
			var refreshToken = new RefreshToken
			{
				UserId = user.Id,
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				TokenCreated = DateTime.Now,
				TokenExpires = DateTime.Now.AddHours(1)
			};
			return refreshToken;
		}


		private void SetRefreshToken(RefreshToken refreshToken)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = refreshToken.TokenExpires
			};
			Response.Cookies.Append("rf", refreshToken.Token, cookieOptions);
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				//new Claim(ClaimTypes.UserData, user.DepartmentId),
				new Claim(ClaimTypes.Name, user.UserName),
				//new Claim(ClaimTypes.Email, user.Email),

			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
				configuration.GetSection("AppSettings:Token").Value));

			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: cred);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}

		private static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			var hmac = new HMACSHA512(passwordSalt);
			var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(passwordHash);
		}

		[HttpPost("Refresh-Token")]
		public async Task<ActionResult<string>> RefreshToken()
		{
			var refreshToken = Request.Cookies["rf"];

			var refreshTokenInDb = dbContext.RefreshTokens.Include(u => u.User)
				.FirstOrDefault(t => t.Token == refreshToken);


			 if (refreshTokenInDb == null)
			{
				return Unauthorized("Invalid Refresh Token");
			}
			else if(refreshTokenInDb.TokenExpires < DateTime.Now)
			{
				return Unauthorized("Token expired");
			}
			var user = refreshTokenInDb.User;
			string token = CreateToken(user);
			var newRefreshToken = GenerateRefreshToken();

			refreshTokenInDb.Token = newRefreshToken.Token;
			refreshTokenInDb.TokenCreated = newRefreshToken.TokenCreated;
			refreshTokenInDb.TokenExpires = newRefreshToken.TokenExpires;
			await dbContext.SaveChangesAsync();

			SetRefreshToken(newRefreshToken);
			
			return Ok(token);
		}
	}
}
