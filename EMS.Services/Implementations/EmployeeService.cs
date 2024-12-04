using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Mappers;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService>logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task AddEmployee(AddEmployeeDto dto)
    {
        Employee employee = dto.MapEmployeeAddDto();

        try
        {
            await _employeeRepository.AddAsync(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(AddEmployee)} function");
            throw;
        }
    }

    public async Task DeleteEmployee(int id)
    {
        try
        {
            await _employeeRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(DeleteEmployee)} function");
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeDetails>?> GetAllEmployees()
    {
        try
        {
            var result = await _employeeRepository.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(GetAllEmployees)} function");
            throw;
        }
    }

    public async Task<EmployeeDetails?> GetEmployeeById(int id)
    {
        try
        {
            var result = await _employeeRepository.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(GetAllEmployees)} function");
            throw;
        }
    }

    public async Task UpdateEmployee(int id, UpdateEmployeeDto dto)
    {
        Employee employee = dto.MapEmployeeUpdateDto();
        employee.Id = id;

        try
        {
            await _employeeRepository.UpdateAsync(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(UpdateEmployee)} function");
            throw;
        }
    }
}
