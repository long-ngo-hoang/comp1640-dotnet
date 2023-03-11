using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using comp1640_dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class ProfilesController : ControllerBase
	{
		private readonly IProfileRepository _profileRepos;
		private readonly IS3Service _s3Service;

		public ProfilesController(IProfileRepository profileRepos, IS3Service s3Service)
		{
			_profileRepos = profileRepos;
			_s3Service = s3Service;
		}

		[HttpGet]
		public async Task<ActionResult<ProfileResponse>> GetProfile()
		{
			var result = await _profileRepos.GetProfile();
			if(result == null)
			{
				return BadRequest("Profile not found");
			}
			return Ok(result);
		}

		[Authorize(Roles = "Administrator")]
		[HttpGet("{userId}")]
		public async Task<ActionResult<ProfileResponse>> GetProfileByUserId(string userId)
		{
			var result = await _profileRepos.GetProfileByUserId(userId);
			if(result == null)
			{
				return BadRequest("Profile not found");
			}
			return Ok(result);
		}

		[Authorize(Roles = "Administrator")]
		[HttpPut("{userId}")]
		public async Task<ActionResult<ProfileResponse>> UpdateProfileByUserId(string userId, ProfileRequest profile)
		{
			var result = await _profileRepos.UpdateProfileByUserId(userId, profile);
			if (result == null)
			{
				return BadRequest("Profile not found");
			}
			return Ok("Update successful profile");
		}

		[HttpGet("GetPreSignedUrlToUploadAvatar")]
		[Authorize]
		public async Task<ActionResult<PreSignedUrlResponse>> GetS3PreSignedUrl()
		{
			var result = await _s3Service.GetPreSignedUrl("avatar-user/");

			return result;
		}
	}
}
