using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
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
		private readonly ApplicationDbContext? _dbContext;
		private readonly IConfiguration _configuration;
		private readonly IProfileRepository _profileRepository;

		public AuthController(ApplicationDbContext? dbContext, IConfiguration configuration, IProfileRepository profileRepository)
		{
			_dbContext = dbContext;
			_configuration = configuration;
			_profileRepository = profileRepository;
		}

		[HttpPost("Register")]
		public async Task<ActionResult<User>> Register(UserRegisterRequest userRegisterRequest)
		{
			var emailInDb = _dbContext.Users
				.SingleOrDefault(u => u.Email.ToLower() == userRegisterRequest.Email.ToLower());

			if (emailInDb == null)
			{
				var hmac = new HMACSHA512();
				User user = new()
				{
					UserName = userRegisterRequest.Email,
					Email = userRegisterRequest.Email,
					DepartmentId = userRegisterRequest.DepartmentId,
					PasswordSalt = PasswordSalt(hmac),
					PasswordHash = PasswordHash(userRegisterRequest.Password, hmac),
				};

				var result = _dbContext.Users.Add(user);

				if (result != null)
				{
					UserRole userRole = new()
					{
						UserId = result.Entity.Id,
						RoleId = "c7b013f0-5201-4317-abd8-c878f91b1111"
					};
					var result1 = await _dbContext.UserRoles.AddAsync(userRole);
					var result2 = await _profileRepository.CreateProfile(result.Entity.Id);
					await _dbContext.SaveChangesAsync();

					return Ok("Account successfully created");
				}
				return BadRequest("Can't create an account");
			}
			return BadRequest("This email is already registered.");
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
			var user = _dbContext.Users
				.FirstOrDefault(u => u.Email == userLoginRequest.Email);

			if (user == null)
			{
				return BadRequest("Account Not Found.");
			}
			if (!VerifyPassword(userLoginRequest.Password, user.PasswordHash, user.PasswordSalt))
			{
				return BadRequest("Wrong Password");
			}

			var refreshTokenInDB = _dbContext.RefreshTokens.SingleOrDefault(u => u.UserId == user.Id);

			string token = CreateToken(user);

			var refreshToken = GenerateRefreshToken(user);

			if(refreshTokenInDB == null)
			{
				_dbContext.RefreshTokens.Add(refreshToken);
			}
			else
			{
				refreshTokenInDB.Token = refreshToken.Token;
				refreshTokenInDB.TokenCreated = refreshToken.TokenCreated;
				refreshTokenInDB.TokenExpires = refreshToken.TokenExpires;
			}
			await _dbContext.SaveChangesAsync();

			SetRefreshToken(refreshToken);

			return Ok(token);
		}

		private static RefreshToken GenerateRefreshToken(User user)
		{
			var refreshToken = new RefreshToken
			{
				UserId = user.Id,
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				UpdatedAt = DateTime.Now,
				TokenCreated = DateTime.Now,
				TokenExpires = DateTime.Now.AddDays(1)
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
			var userRoleInDb = _dbContext.UserRoles
				.Include(r => r.Role)
				.Include(u => u.User)
				.SingleOrDefault(u => u.UserId == user.Id);

			var academicYearInDb = _dbContext.AcademicYears.OrderByDescending(p => p.StartDate).Last();
			var departmentInDb = userRoleInDb.User.DepartmentId;

			List<Claim> claims = new List<Claim>
			{
				new Claim("AcademicYearId", academicYearInDb.Id),
				new Claim("DepartmentId", departmentInDb),
				new Claim("UserId", user.Id),
				new Claim("UserName", user.UserName),
				new Claim(ClaimTypes.Role, userRoleInDb.Role.Name), 
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
				_configuration.GetSection("AppSettings:Token").Value));

			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddMinutes(60),
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

			var refreshTokenInDb = _dbContext.RefreshTokens.Include(u => u.User)
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
			var newRefreshToken = GenerateRefreshToken(user);

			refreshTokenInDb.Token = newRefreshToken.Token;
			refreshTokenInDb.TokenCreated = newRefreshToken.TokenCreated;
			refreshTokenInDb.TokenExpires = newRefreshToken.TokenExpires;
			await _dbContext.SaveChangesAsync();

			SetRefreshToken(newRefreshToken);
			
			return Ok(token);
		}
	}
}
