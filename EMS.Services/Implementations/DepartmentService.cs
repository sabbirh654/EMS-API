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

    public DepartmentService(IDepartmentRepository departmentRepository, ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task AddDepartment(AddUpdateDepartmentDto dto)
    {
        Department department = dto.MapDepartmentAddUpdateDto();

        try
        {
            await _departmentRepository.AddAsync(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DepartmentService)} at {nameof(AddDepartment)} function");
            throw;
        }
    }

    public async Task DeleteDepartment(int id)
    {
        try
        {
            await _departmentRepository.DeleteAsync(id);
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

        try
        {
            await _departmentRepository.UpdateAsync(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DepartmentService)} at {nameof(UpdateDepartment)} function");
            throw;
        }
    }
}
