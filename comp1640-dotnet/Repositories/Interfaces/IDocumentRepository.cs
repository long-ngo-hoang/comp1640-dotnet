using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IDocumentRepository
	{
		Task<IEnumerable<Document>> GetDocuments();
		Task<Document> GetDocument(string idDocument);
		Task<Document> CreateDocument(Document document);
		Task<Document> RemoveDocument (string idDocument);
		Task<Document> UpdateDocument(string idDocument, Document document);
	}
}
