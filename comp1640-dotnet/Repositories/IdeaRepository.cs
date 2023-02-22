using Amazon.S3.Model;
using Amazon.S3;
using comp1640_dotnet.Data;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Amazon;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.Runtime;
using comp1640_dotnet.DTOs.Responses;

namespace comp1640_dotnet.Repositories
{
	public class IdeaRepository : IIdeaRepository
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IConfiguration configuration;


		public IdeaRepository(ApplicationDbContext context, IConfiguration configuration)
		{
			dbContext = context;
			this.configuration = configuration;

		}

		public async Task<Idea> CreateIdea(Idea idea)
		{
			var result = await dbContext.Ideas.AddAsync(idea);
			await dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Idea> GetIdea(string idIdea)
		{
			return dbContext.Ideas.SingleOrDefault(i => i.Id == idIdea);
		}

		public async Task<IEnumerable<Idea>> GetIdeas()
		{
			return await dbContext.Ideas.ToListAsync();
		}

		public async Task<Idea> RemoveIdea(string idIdea)
		{
			var result = await dbContext.Ideas
							 .SingleOrDefaultAsync(e => e.Id == idIdea);

			if (result != null)
			{
				dbContext.Ideas.Remove(result);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}

		public async Task<Idea> UpdateIdea(string idIdea, Idea idea)
		{
			var ideaInDb = await dbContext.Ideas
							 .SingleOrDefaultAsync(e => e.Id == idIdea);

			if (ideaInDb != null)
			{
				ideaInDb.Name = idea.Name;
				ideaInDb.Description = idea.Description;
				ideaInDb.IsAnonymous = idea.IsAnonymous;
				await dbContext.SaveChangesAsync();
			}
			return ideaInDb;
		}

		public PreSignedUrlResponse GetS3PreSignedUrl()
		{
			var amazonAccessKey = configuration["AWS:ACCESS_KEY"];
			var amazonSecretKey = configuration["AWS:SECRET_KEY"];
			var amazonBucketName = configuration["AWS:BUCKET_NAME"];
			string fileName = Guid.NewGuid().ToString();

			IAmazonS3 client = new AmazonS3Client(amazonAccessKey, amazonSecretKey, RegionEndpoint.APSoutheast1);

			GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
			{
				BucketName = amazonBucketName,
				Key = fileName + ".jpg",
				Expires = DateTime.Now.AddMinutes(20),
				Verb = HttpVerb.PUT,
			};

			string preSignedUrl = client.GetPreSignedURL(request);

			PreSignedUrlResponse preSignedUrlResponse = new PreSignedUrlResponse
			{
				FileName = fileName + ".jpg",
				PreSignedUrl = preSignedUrl,
			};
			return preSignedUrlResponse;
		}
	}
}
