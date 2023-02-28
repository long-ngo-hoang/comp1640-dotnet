using comp1640_dotnet.Data;
using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Factory;
using comp1640_dotnet.Models;
using comp1640_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace comp1640_dotnet.Repositories
{
	public class AcademicYearRepository : IAcademicYearRepository
	{
		private readonly ApplicationDbContext dbContext;
		private readonly ConvertFactory convertFactory;
		private readonly int pageSize = 5;

		public AcademicYearRepository(ApplicationDbContext context, ConvertFactory _convertFactory)
		{
			dbContext = context;
			convertFactory = _convertFactory;
		}

		public async Task<AcademicYearResponse> CreateAcademicYear(AcademicYearRequest academicYear)
{
			AcademicYear academicYearToCreate = new()
			{
				Name = academicYear.Name,
				StartDate = academicYear.StartDate,
				ClosureDate = academicYear.ClosureDate,
				FinalClosureDate = academicYear.FinalClosureDate
			};

			var result = await dbContext.AcademicYears.AddAsync(academicYearToCreate);
			await dbContext.SaveChangesAsync();

			AcademicYearResponse academicYearResponse = new()
			{
				Id = result.Entity.Id,
				Name = result.Entity.Name,
				StartDate = result.Entity.StartDate,
				ClosureDate = result.Entity.ClosureDate,
				FinalClosureDate = result.Entity.FinalClosureDate
			};

			return academicYearResponse;
		}

		public async Task<AcademicYearResponse> GetAcademicYear(string idAcademicYear, int pageIndex)
		{
			var academicYearInDb = dbContext.AcademicYears
				.Include(i => i.Ideas
					.Skip((pageIndex - 1) * pageSize)
					.Take(pageSize))
				.SingleOrDefault(i => i.Id == idAcademicYear);

			int academicYearsCount = dbContext.Ideas
				.Where(i => i.AcademicYearId == idAcademicYear)
				.Count();

			AcademicYearResponse academicYearResponse = new()
			{
				Id = academicYearInDb.Id,
				CreatedAt = academicYearInDb.CreatedAt,
				UpdatedAt = academicYearInDb.UpdatedAt,
				Name = academicYearInDb.Name,
				StartDate = academicYearInDb.StartDate,
				ClosureDate = academicYearInDb.ClosureDate,
				FinalClosureDate = academicYearInDb.FinalClosureDate,
				AllIdeas = new AllIdeasResponse()
				{
					PageIndex = pageIndex,
					TotalPage = (int)Math.Ceiling((double)academicYearsCount / pageSize),
					Ideas = convertFactory.ConvertListIdeas(academicYearInDb.Ideas)
				}
			};
			return academicYearResponse;
		}

		public async Task<IEnumerable<AcademicYear>> GetAcademicYears()
		{
			return await dbContext.AcademicYears.ToListAsync();
		}

		public async Task<AcademicYear> RemoveAcademicYear(string idAcademicYear)
		{
			var result = await dbContext.AcademicYears
							 .SingleOrDefaultAsync(e => e.Id == idAcademicYear);

			if (result != null)
			{
				dbContext.AcademicYears.Remove(result);
				await dbContext.SaveChangesAsync();
			}
			return result;
		}

		public async Task<AcademicYearResponse?> UpdateAcademicYear(string idAcademicYear, AcademicYearRequest academicYear)
		{
			var academicYearInDb = await dbContext.AcademicYears
							 .SingleOrDefaultAsync(e => e.Id == idAcademicYear);

			AcademicYearResponse academicYearResponse = new();

			if (academicYearInDb == null)
			{
				return null;
			}
			else
			{
				academicYearInDb.Name = academicYear.Name;
				academicYearInDb.StartDate = academicYear.StartDate;
				academicYearInDb.ClosureDate = academicYear.ClosureDate;
				academicYearInDb.FinalClosureDate = academicYear.FinalClosureDate;

				await dbContext.SaveChangesAsync();

				academicYearResponse.Id = academicYearInDb.Id;
				academicYearResponse.CreatedAt = academicYearInDb.CreatedAt;
				academicYearResponse.UpdatedAt = academicYearInDb.UpdatedAt;
				academicYearResponse.Name = academicYearInDb.Name;
				academicYearResponse.StartDate = academicYearInDb.StartDate;
				academicYearResponse.ClosureDate = academicYearInDb.ClosureDate;
				academicYearResponse.FinalClosureDate = academicYearInDb.FinalClosureDate;
			}

			return academicYearResponse;
		}
	}
}
