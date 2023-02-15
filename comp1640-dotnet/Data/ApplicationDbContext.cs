using comp1640_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}
		public DbSet<Department>? Departments { get; set; }
		public DbSet<AcademicYear>? AcademicYears { get; set; }
		public DbSet<Category>? Categories { get; set; }
		public DbSet<Idea>? Ideas { get; set; }
		public DbSet<Reaction>? Reactions { get; set; }
		public DbSet<Comment>? Comments { get; set; }
		public DbSet<Document>? Documents { get; set; }
		public DbSet<Invitation>? Invations { get; set; }
	}
}
