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
    private readonly IOperationLogRepository _operationLogRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService>logger, IOperationLogRepository operationLogRepository)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _operationLogRepository = operationLogRepository;
    }

    public async Task AddEmployee(AddEmployeeDto dto)
    {
        Employee employee = dto.MapEmployeeAddDto();
        OperationLog log = new OperationLog("ADD", "Employee", "", "New employee has been added");

        try
        {
            await _employeeRepository.AddAsync(employee);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(AddEmployee)} function");
            throw;
        }
    }

    public async Task DeleteEmployee(int id)
    {
        OperationLog log = new OperationLog("DELETE", "Employee", $"{id}", $"An employee has been deleted with Id = {id}");

        try
        {
            await _employeeRepository.DeleteAsync(id);
            await _operationLogRepository.AddLogAsync(log);
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

        OperationLog log = new ("UPDATE", "Employee", $"{id}", $"An employee has been updated with Id = {id}");

        try
        {
            await _employeeRepository.UpdateAsync(employee);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(UpdateEmployee)} function");
            throw;
        }
    }
}
