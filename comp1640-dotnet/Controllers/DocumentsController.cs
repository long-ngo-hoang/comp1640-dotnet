﻿using ClosedXML.Excel;
using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace comp1640_dotnet.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DocumentsController : ControllerBase
	{
		private readonly IDocumentRepository _documentRepos;
		private readonly ApplicationDbContext _dbContext;

		public DocumentsController(IDocumentRepository documentRepos, ApplicationDbContext dbContext)
		{
			_documentRepos = documentRepos;
			_dbContext = dbContext;
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

		[HttpGet("DownloadZip")]
		public async Task<ActionResult> DownloadZip()
		{
			var documentsInDb = await _documentRepos.GetDocuments();
			var ideas = _dbContext.Ideas.ToList();

			var client = new HttpClient();

			var zipName = "Documents.zip";
			if (documentsInDb != null)
			{
				MemoryStream ms = new();

				using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
				{

					foreach (var document in documentsInDb)
					{
						var result = await client.GetAsync(document.DocumentUrl);
						byte[] bytes = await result.Content.ReadAsByteArrayAsync();

						var entry = zip.CreateEntry("Documents/" + document.Id + ".jpg");
						using var fileStream = new MemoryStream(bytes);
						using var entryStream = entry.Open();
						fileStream.CopyTo(entryStream);
						entryStream.Close();
					}
				
					using (IXLWorkbook workbook = new XLWorkbook())
					{					
						workbook.AddWorksheet("Ideas").FirstCell().InsertTable<Idea>(ideas, false);
						workbook.SaveAs("Ideas.xlsx");
					}
					zip.CreateEntryFromFile(Path.Combine("Ideas.xlsx"), "Ideas/Ideas.xlsx");
				}			

				return File(ms.ToArray(), "application/zip", zipName);

			}
			return BadRequest();
		}
	}
}
