using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Mappers;
using EMS.Core.Models;
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

    public async Task<ApiResult> AddDepartment(AddUpdateDepartmentDto dto)
    {
        Department department = dto.MapDepartmentAddUpdateDto();

        try
        {
            return await _departmentRepository.AddAsync(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentService), nameof(AddDepartment), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_DEPARTMENT_ERROR);
        }
    }

    public async Task<ApiResult> DeleteDepartment(int id)
    {
        try
        {
            var department = _departmentRepository.GetByIdAsync(id);

            if (department.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Department not found or deleted");
            }

            return await _departmentRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentService), nameof(DeleteDepartment), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DEPARTMENT_ERROR);
        }
    }

    public async Task<ApiResult> GetAllDepartments()
    {
        try
        {
            return await _departmentRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentService), nameof(GetAllDepartments), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
        }
    }

    public async Task<ApiResult> GetDepartmentById(int id)
    {
        try
        {
            var department = _departmentRepository.GetByIdAsync(id);

            if (department.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Department not found or deleted");
            }

            return await _departmentRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentService), nameof(GetDepartmentById), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
        }
    }

    public async Task<ApiResult> UpdateDepartment(int id, AddUpdateDepartmentDto dto)
    {
        Department department = dto.MapDepartmentAddUpdateDto();
        department.Id = id;

        try
        {
            var dept = _departmentRepository.GetByIdAsync(id);

            if (dept.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Department not found or deleted");
            }

            return await _departmentRepository.UpdateAsync(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentService), nameof(UpdateDepartment), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DEPARTMENT_ERROR);
        }
    }
}
