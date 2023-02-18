using comp1640_dotnet.Data;
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

		public async Task<Document> CreateDocument(Document document)
{
			var result = await dbContext.Documents.AddAsync(document);
			await dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Document> GetDocument(string idDocument)
		{
			return dbContext.Documents.SingleOrDefault(i => i.Id == idDocument);
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

		public async Task<Document> UpdateDocument(string idDocument, Document document)
		{
			var documentInDb = await dbContext.Documents
							 .SingleOrDefaultAsync(e => e.Id == idDocument);

			if (documentInDb != null)
			{
				documentInDb.DocumentUrl = document.DocumentUrl;
				await dbContext.SaveChangesAsync();
			}
			return documentInDb;
		}
	}
}
