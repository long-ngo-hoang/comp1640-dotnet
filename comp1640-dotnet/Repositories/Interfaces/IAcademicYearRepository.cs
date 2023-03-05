using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IAcademicYearRepository
	{
		Task<IEnumerable<AcademicYear>> GetAcademicYears();
		Task<AcademicYearResponse> GetAcademicYear(string idAcademicYear, int pageIndex);
		Task<AcademicYearResponse> CreateAcademicYear(AcademicYearRequest academicYear);	
		Task<AcademicYearResponse> UpdateAcademicYear(string idAcademicYear, AcademicYearRequest academicYear);
		Task<AcademicYear> RemoveAcademicYear(string idAcademicYear);
		Task<bool> CheckDeadlineForNewIdeas();
		Task<bool> CheckDeadlineForNewComments();
	}
}