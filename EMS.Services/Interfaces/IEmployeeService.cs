using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDetails>?> GetAllEmployees();
    Task<EmployeeDetails?> GetEmployeeById(int id);
    Task AddEmployee(AddEmployeeDto employee);
    Task UpdateEmployee(int id, UpdateEmployeeDto employee);
    Task DeleteEmployee(int id);
}
