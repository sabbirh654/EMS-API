using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Mappers;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<DepartmentService> _logger;
    private readonly IOperationLogRepository _operationLogRepository;

    public DepartmentService(IDepartmentRepository departmentRepository, ILogger<DepartmentService> logger, IOperationLogRepository operationLogRepository)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
        _operationLogRepository = operationLogRepository;
    }

    public async Task AddDepartment(AddUpdateDepartmentDto dto)
    {
        Department department = dto.MapDepartmentAddUpdateDto();

        OperationLog log = new OperationLog("ADD", "Department", "", "New department has been added");

        try
        {
            await _departmentRepository.AddAsync(department);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DepartmentService)} at {nameof(AddDepartment)} function");
            throw;
        }
    }

    public async Task DeleteDepartment(int id)
    {
        OperationLog log = new OperationLog("DELETE", "Department", $"{id}", $"A department has been deleted with Id = {id}");

        try
        {
            await _departmentRepository.DeleteAsync(id);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DepartmentService)} at {nameof(DeleteDepartment)} function");
            throw;
        }
    }

    public async Task<IEnumerable<Department>?> GetAllDepartments()
    {
        try
        {
            var result = await _departmentRepository.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DepartmentService)} at {nameof(GetAllDepartments)} function");
            throw;
        }
    }

    public async Task<Department?> GetDepartmentById(int id)
    {
        try
        {
            var result = await _departmentRepository.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DepartmentService)} at {nameof(GetDepartmentById)} function");
            throw;
        }
    }

    public async Task UpdateDepartment(int id, AddUpdateDepartmentDto dto)
    {
        Department department = dto.MapDepartmentAddUpdateDto();
        department.Id = id;

        OperationLog log = new("UPDATE", "Department", $"{id}", $"A department has been updated with Id = {id}");

        try
        {
            await _departmentRepository.UpdateAsync(department);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DepartmentService)} at {nameof(UpdateDepartment)} function");
            throw;
        }
    }
}
