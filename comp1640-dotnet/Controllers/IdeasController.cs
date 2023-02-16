//using comp1640_dotnet.Data;
//using comp1640_dotnet.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace comp1640_dotnet.Controllers
//{
//	[Route("api/[controller]")]
//	[ApiController]
//	public class IdeasController : ControllerBase
//	{
//		private readonly ApplicationDbContext? dbContext;
//		public IdeasController(ApplicationDbContext? dbContext)
//		{
//			this.dbContext = dbContext;
//		}

//		[HttpGet]
//		public async Task<ActionResult<IEnumerable<Idea>>> GetIdeas()
//		{
//			return await dbContext.Ideas.ToListAsync();
//		}

//		[HttpPost]
//		public async Task<ActionResult<Idea>> CreateIdea(Idea idea)
//		{
//			dbContext.Ideas.Add(idea);
//			await dbContext.SaveChangesAsync();

//			return Ok("Created a successful idea");
//		}

//		[HttpPut("id")]
//		public async Task<ActionResult<Idea>> CreateIdea(Guid id, Idea idea)
//		{
//			if (id != idea.Id)
//			{
//				return BadRequest();
//			}
//			 dbContext.Ideas.Update(idea);

//			await dbContext.SaveChangesAsync();

//			return Ok("Updated a successful idea");
//		}

//	}
//}
