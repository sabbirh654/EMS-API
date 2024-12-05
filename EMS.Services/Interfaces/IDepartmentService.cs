using EMS.Core.DTOs;
using EMS.Core.Models;

namespace EMS.Services.Interfaces;

public interface IDepartmentService
{
    Task<ApiResult> GetAllDepartments();
    Task<ApiResult> GetDepartmentById(int id);
    Task<ApiResult> AddDepartment(AddUpdateDepartmentDto dto);
    Task<ApiResult> UpdateDepartment(int id, AddUpdateDepartmentDto dto);
    Task<ApiResult> DeleteDepartment(int id);
}
