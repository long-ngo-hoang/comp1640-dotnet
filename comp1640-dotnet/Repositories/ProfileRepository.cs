using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace comp1640_dotnet.Repositories
{
	public class ProfileRepository : IProfileRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProfileRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = context;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<Profile>? CreateProfile(string userId)
		{
			Profile profileToCreate = new()
			{
				UserId = userId,
				AvatarUrl = "New Users",
				FullName = "New Users",
				Address = "New Users",
				Phone = "New Users"
			};

			var result = await _dbContext.Profiles.AddAsync(profileToCreate);
			await _dbContext.SaveChangesAsync();

			if(result == null)
			{
				return null;
			}
			return result.Entity;
		}

		public async Task<ProfileResponse> GetProfile()
{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");

			var profileInDb = _dbContext.Profiles.SingleOrDefault(i => i.UserId == userId);

			ProfileResponse profileResponse = new()
			{
				Id = profileInDb.Id,
				CreatedAt = profileInDb.CreatedAt,
				UpdatedAt = profileInDb.UpdatedAt,
				AvatarUrl = profileInDb.AvatarUrl,
		    FullName = profileInDb.FullName,
				Address = profileInDb.Address,
		    Phone = profileInDb.Phone
			};
			return profileResponse;
		}

		public async Task<ProfileResponse?> GetProfileByUserId(string userId)
		{
			var profileInDb = await _dbContext.Profiles
										 .SingleOrDefaultAsync(e => e.UserId == userId);

			if(profileInDb == null)
			{
				return null;
			}

			ProfileResponse profileResponse = new()
			{
				Id = profileInDb.Id,
				CreatedAt = profileInDb.CreatedAt,
				UpdatedAt = profileInDb.UpdatedAt,
				AvatarUrl = profileInDb.AvatarUrl,
				FullName = profileInDb.FullName,
				Address = profileInDb.Address,
				Phone = profileInDb.Phone,
			};
			return profileResponse;
		}

		public async Task<IEnumerable<Profile>> GetProfiles()
		{
			return await _dbContext.Profiles.ToListAsync();
		}

		public async Task<ProfileResponse?> UpdateProfileByUserId(string userId, ProfileRequest profile)
		{
			var profileInDb = await _dbContext.Profiles
							 .SingleOrDefaultAsync(e => e.UserId == userId);

			ProfileResponse profileResponse = new();

			if (profileInDb == null)
			{
				return null;
			}
			else
			{
				profileInDb.AvatarUrl = profile.AvatarUrl;
				profileInDb.FullName = profile.FullName;
				profileInDb.Address = profile.Address;
				profileInDb.Phone = profile.Phone;

				await _dbContext.SaveChangesAsync();

				profileResponse.Id = profileInDb.Id;
				profileResponse.CreatedAt = profileInDb.CreatedAt;
				profileResponse.UpdatedAt = profileInDb.UpdatedAt;
				profileResponse.AvatarUrl = profileInDb.AvatarUrl;
				profileResponse.FullName = profileInDb.FullName;
				profileResponse.Address = profileInDb.Address;
				profileResponse.Phone = profileInDb.Phone;
			}

			return profileResponse;
		}
	}
}
