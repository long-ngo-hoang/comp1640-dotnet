using comp1640_dotnet.DTOs.Requests;
using comp1640_dotnet.DTOs.Responses;
using comp1640_dotnet.Models;

namespace comp1640_dotnet.Repositories.Interfaces
{
	public interface IDepartmentRepository
	{
		Task<IEnumerable<Department>> GetDepartments();
		Task<DepartmentResponse> GetDepartment(string idDepartment, int pageIndex);
		Task<DepartmentResponse> CreateDepartment(DepartmentRequest department);
	}
}
