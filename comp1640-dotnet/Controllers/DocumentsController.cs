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
	[Authorize]
	public class DocumentsController : ControllerBase
	{
		private readonly IDocumentRepository _documentRepos;

		public DocumentsController(IDocumentRepository documentRepos)
		{
			_documentRepos = documentRepos;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
		{
			var result = await _documentRepos.GetDocuments();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<DocumentResponse>> GetDocument(string id)
		{
			var result = await _documentRepos.GetDocument(id);
			if(result == null)
			{
				return BadRequest("Document not found");
			}
			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "Staff")]
		public async Task<ActionResult<DocumentResponse>> CreateDocument(DocumentRequest document)
		{
			var result = await _documentRepos.CreateDocument(document);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Staff")]
		public async Task<ActionResult<Document>> RemoveDocument(string id)
		{
			var result = await _documentRepos.RemoveDocument(id);
			if(result == null)
			{
				return BadRequest("Document not found");
			}
			return Ok("Delete successful document");
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Staff")]
		public async Task<ActionResult<DocumentResponse>> UpdateDocument(string id, DocumentRequest document)
		{
			var result = await _documentRepos.UpdateDocument(id, document);
			if (result == null)
			{
				return BadRequest("Document not found");
			}
			return Ok("Update successful document");
		}
	}
}
