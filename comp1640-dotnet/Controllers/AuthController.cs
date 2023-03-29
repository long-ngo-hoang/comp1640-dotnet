using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using comp1640_dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
		private readonly IProfileRepository _profileRepos;
		private readonly IEmailService _emailService;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public AuthController(ApplicationDbContext? dbContext, 
			IConfiguration configuration, 
			IProfileRepository profileRepos, 
			IEmailService emailService,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_configuration = configuration;
			_profileRepos = profileRepos;
			_emailService = emailService;
			_httpContextAccessor = httpContextAccessor;
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
					var result2 = await _profileRepos.CreateProfile(result.Entity.Id);
					await _dbContext.SaveChangesAsync();

					return Ok("Account successfully created");
				}
				return BadRequest("Can't create an account");
			}
			return BadRequest("This email is already registered.");
		}
		
		[HttpPost("ForgotPassword")]
		public async Task<ActionResult<User>> ForgotPassword(string email)
		{
			var emailInDb = _dbContext.Users
				.SingleOrDefault(u => u.Email.ToLower() == email.ToLower());

			if (emailInDb != null)
			{
				Random rnd = new ();
				int newPassword = rnd.Next(100000, 999999);

				var hmac = new HMACSHA512();

				emailInDb.PasswordSalt = PasswordSalt(hmac);
				emailInDb.PasswordHash = PasswordHash(newPassword.ToString(), hmac);

				await _dbContext.SaveChangesAsync();

				_emailService.SendEmail(email, "Password recovery successful",  "Your new password is:" + newPassword);

				return Ok("New password has been sent to your email address. Please use a new password.");
			};

			return BadRequest("Email not found");
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
		public async Task<ActionResult<AuthResponse>> Login(UserLoginRequest userLoginRequest)
		{
			var user = _dbContext.Users
				.FirstOrDefault(u => u.Email == userLoginRequest.Email);

			if (user == null)
			{
				return NotFound("Account Not Found");
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

			AuthResponse authResponse = new()
			{
				Token = token,
				RefreshToken = refreshToken.Token
			};
			return Ok(authResponse);
		}
		
		[HttpPost("ChangePassword")]
		[Authorize]
		public async Task<ActionResult> ChangePassword (ChangePasswordRequest changePasswordRequest)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var user = _dbContext.Users
				.FirstOrDefault(u => u.Id == userId);

			if (user == null)
			{
				return BadRequest("Account Not Found");
			}
			if (!VerifyPassword(changePasswordRequest.Password, user.PasswordHash, user.PasswordSalt))
			{
				return BadRequest("Wrong Password");
			}
			var hmac = new HMACSHA512();

			user.PasswordSalt = PasswordSalt(hmac);
			user.PasswordHash = PasswordHash(changePasswordRequest.NewPassword, hmac);

			await _dbContext.SaveChangesAsync();

			return Ok("Change password successfully.");
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

		private string CreateToken(User user)
		{
			var userRoleInDb = _dbContext.UserRoles
				.Include(r => r.Role)
				.Include(u => u.User)
				.SingleOrDefault(u => u.UserId == user.Id);
			var academicYearInDb = _dbContext.AcademicYears.OrderByDescending(p => p.StartDate).FirstOrDefault();
			var departmentInDb = userRoleInDb.User.DepartmentId;

			List<Claim> claims = new();

			if (userRoleInDb.Role.Name == "Staff" || userRoleInDb.Role.Name == "Quality Assurance Coordinator")
			{
				claims.Add(new Claim("AcademicYearId", academicYearInDb.Id));
				claims.Add(new Claim("DepartmentId", departmentInDb));
				claims.Add(new Claim("UserId", user.Id));
				claims.Add(new Claim("UserName", user.UserName));
				claims.Add(new Claim(ClaimTypes.Role, userRoleInDb.Role.Name));
				claims.Add(new Claim("Roles", userRoleInDb.Role.Name));
			}
			else
			{
				claims.Add(new Claim("UserId", user.Id));
				claims.Add(new Claim("UserName", user.UserName));
				claims.Add(new Claim(ClaimTypes.Role, userRoleInDb.Role.Name));
				claims.Add(new Claim("Roles", userRoleInDb.Role.Name));
			}

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

		[HttpPost("RefreshToken")]
		[Authorize]
		public async Task<ActionResult<AuthResponse>> RefreshToken(string refreshToken)
		{
			var refreshTokenInDb = _dbContext.RefreshTokens.Include(u => u.User)
				.FirstOrDefault(t => t.Token == refreshToken);

			if (refreshTokenInDb == null)
			{
				return Unauthorized("Invalid Refresh Token");
			}
			else if(refreshTokenInDb.TokenExpires < DateTime.Now)
			{
				return Unauthorized("Refresh Token expired");
			}		

			var user = refreshTokenInDb.User;

			string token = CreateToken(user);
			var newRefreshToken = GenerateRefreshToken(user);

			refreshTokenInDb.Token = newRefreshToken.Token;
			refreshTokenInDb.TokenCreated = newRefreshToken.TokenCreated;
			refreshTokenInDb.TokenExpires = newRefreshToken.TokenExpires;
			await _dbContext.SaveChangesAsync();

			AuthResponse authResponse = new()
			{
				Token = token,
				RefreshToken = newRefreshToken.Token
			};
			
			return Ok(authResponse);
		}
	}
}
