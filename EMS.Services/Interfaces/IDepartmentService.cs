using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<Department>?> GetAllDepartments();
    Task<Department?> GetDepartmentById(int id);
    Task AddDepartment(AddUpdateDepartmentDto dto);
    Task UpdateDepartment(int id, AddUpdateDepartmentDto dto);
    Task DeleteDepartment(int id);
}
