using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Mappers;
using EMS.Core.Models;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDesignationRepository _designationRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService>logger, IDepartmentRepository departmentRepository, IDesignationRepository designationRepository)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _departmentRepository = departmentRepository;
        _designationRepository = designationRepository;
    }

    public async Task<ApiResult> AddEmployee(AddEmployeeDto dto)
    {
        Employee employee = dto.MapEmployeeAddDto();

        try
        {
            var department = _departmentRepository.GetByIdAsync(employee.DepartmentId);

            if (department.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Department not found or deleted");
            }

            var designation = _designationRepository.GetByIdAsync(employee.DesignationId);

            if (designation.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Designation not found or deleted");
            }

            var apiResult = await _employeeRepository.AddAsync(employee);
            return apiResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeeService), nameof(AddEmployee), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_EMPLOYEE_ERROR);
        }
    }

    public async Task<ApiResult> DeleteEmployee(int id)
    {
        try
        {
            var employee = _employeeRepository.GetByIdAsync(id);

            if (employee.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Employee not found or deleted");
            }

            var apiResult = await _employeeRepository.DeleteAsync(id);
            return apiResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeeService), nameof(DeleteEmployee), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR);
        }
    }

    public async Task<ApiResult> GetAllEmployees()
    {
        try
        {
            var result = await _employeeRepository.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeeService), nameof(GetAllEmployees), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR);
        }
    }

    public async Task<ApiResult> GetEmployeeById(int id)
    {
        try
        {
            var result = await _employeeRepository.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeeService), nameof(GetEmployeeById), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR);
        }
    }

    public async Task<ApiResult> UpdateEmployee(int id, UpdateEmployeeDto dto)
    {
        Employee employee = dto.MapEmployeeUpdateDto();
        employee.Id = id;

        try
        {
            var employeeTobeUpdated = _employeeRepository.GetByIdAsync(id);

            if(employeeTobeUpdated.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Employee not found or deleted");
            }

            var apiResult = await _employeeRepository.UpdateAsync(employee);
            return apiResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeeService), nameof(UpdateEmployee), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR);
        }
    }
}
