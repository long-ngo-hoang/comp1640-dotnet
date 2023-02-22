using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace comp1640_dotnet.Repositories
{
	public class DocumentRepository : IDocumentRepository
	{
		private readonly ApplicationDbContext dbContext;

		public DocumentRepository(ApplicationDbContext context)
		{
			dbContext = context;
		}

		public async Task<DocumentResponse> CreateDocument(DocumentRequest document)
{
			Document documentToCreate = new()
			{
				IdeaId = document.IdeaId,
				DocumentUrl = document.DocumentUrl
			};
			var result = await dbContext.Documents.AddAsync(documentToCreate);
			await dbContext.SaveChangesAsync();

			DocumentResponse documentResponse = new()
			{
				Id = result.Entity.Id,
				IdeaId = result.Entity.IdeaId,
				CreatedAt = result.Entity.CreatedAt,
				UpdatedAt = result.Entity.UpdatedAt,
				DocumentUrl = result.Entity.DocumentUrl
			};
			return documentResponse;
		}

		public async Task<DocumentResponse> GetDocument(string idDocument)
		{
			var documentInDb = dbContext.Documents.SingleOrDefault(i => i.Id == idDocument);

			DocumentResponse documentResponse = new()
			{
				Id = documentInDb.Id,
				IdeaId = documentInDb.IdeaId,
				CreatedAt = documentInDb.CreatedAt,
				UpdatedAt = documentInDb.UpdatedAt,
				DocumentUrl = documentInDb.DocumentUrl
			};
			return documentResponse;
		}

		public async Task<IEnumerable<Document>> GetDocuments()
		{
			return await dbContext.Documents.ToListAsync();
		}

		public async Task<Document> RemoveDocument(string idDocument)
		{
			var result = await dbContext.Documents
							 .SingleOrDefaultAsync(e => e.Id == idDocument);

			if (result != null)
			{
				dbContext.Documents.Remove(result);
				await dbContext.SaveChangesAsync();

			}
			return result;
		}

		public async Task<DocumentResponse?> UpdateDocument(string idDocument, DocumentRequest document)
		{
			var documentInDb = await dbContext.Documents
							 .SingleOrDefaultAsync(e => e.Id == idDocument);
			DocumentResponse documentResponse = new();


			if (documentInDb == null)
			{
				return null;
			}
			else
			{
				documentInDb.DocumentUrl = document.DocumentUrl;
				await dbContext.SaveChangesAsync();

				documentResponse.Id = documentInDb.Id;
				documentResponse.IdeaId = documentInDb.IdeaId;
				documentResponse.CreatedAt = documentInDb.CreatedAt;
				documentResponse.UpdatedAt = documentInDb.UpdatedAt;
				documentResponse.DocumentUrl = documentInDb.DocumentUrl;
			}

			return documentResponse;
		}
	}
}
