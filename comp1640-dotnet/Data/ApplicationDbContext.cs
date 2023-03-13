using comp1640_dotnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace comp1640_dotnet.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}
		public DbSet<User>? Users { get; set; }
		public DbSet<Profile>? Profiles { get; set; }
		public DbSet<Department>? Departments { get; set; }
		public DbSet<AcademicYear>? AcademicYears { get; set; }
		public DbSet<Category>? Categories { get; set; }
		public DbSet<Idea>? Ideas { get; set; }
		public DbSet<Reaction>? Reactions { get; set; }
		public DbSet<Comment>? Comments { get; set; }
		public DbSet<Document>? Documents { get; set; }
		public DbSet<RefreshToken>? RefreshTokens { get; set; }
		public DbSet<Role>? Roles { get; set; }
		public DbSet<UserRole>? UserRoles { get; set; }
		public DbSet<Notification>? Notifications { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Role>().HasData(
					new Role() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Administrator"},
					new Role() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "Quality Assurance Manager"},
					new Role() { Id = "c7b013f0-5201-4317-abd8-c313f91b2220", Name = "Quality Assurance Coordinator"},
					new Role() { Id = "c7b013f0-5201-4317-abd8-c878f91b1111", Name = "Staff" }
			);
		}
	}
}
