using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DocumentsController : ControllerBase
	{
		private readonly IDocumentRepository documentRepos;

		public DocumentsController(IDocumentRepository _documentRepos)
		{
			this.documentRepos = _documentRepos;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
		{
			var result = await documentRepos.GetDocuments();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<DocumentResponse>> GetDocument(string id)
		{
			var result = await documentRepos.GetDocument(id);
			if(result == null)
			{
				return BadRequest("Document not found");
			}
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<DocumentResponse>> CreateDocument(DocumentRequest document)
		{
			var result = await documentRepos.CreateDocument(document);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Document>> RemoveDocument(string id)
		{
			var result = await documentRepos.RemoveDocument(id);
			if(result == null)
			{
				return BadRequest("Document not found");
			}
			return Ok("Delete successful document");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<DocumentResponse>> UpdateDocument(string id, DocumentRequest document)
		{
			var result = await documentRepos.UpdateDocument(id, document);
			if (result == null)
			{
				return BadRequest("Document not found");
			}
			return Ok("Update successful document");
		}
	}
}
