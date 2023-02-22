using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IDocumentRepository
	{
		Task<IEnumerable<Document>> GetDocuments();
		Task<DocumentResponse> GetDocument(string idDocument);
		Task<DocumentResponse> CreateDocument(DocumentRequest document);
		Task<Document> RemoveDocument (string idDocument);
		Task<DocumentResponse> UpdateDocument(string idDocument, DocumentRequest document);
	}
}
