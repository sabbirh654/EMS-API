using EMS.Core.DTOs;
using EMS.Core.Models;

namespace EMS.Services.Interfaces;

public interface IEmployeeService
{
    Task<ApiResult> GetAllEmployees();
    Task<ApiResult> GetEmployeeById(int id);
    Task<ApiResult> AddEmployee(AddEmployeeDto employee);
    Task<ApiResult> UpdateEmployee(int id, UpdateEmployeeDto employee);
    Task<ApiResult> DeleteEmployee(int id);
}
