using EMS.Core.DTOs;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await _employeeService.GetAllEmployees();

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(GetAllEmployees), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var result = await _employeeService.GetEmployeeById(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(GetEmployeeById), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeDto dto)
        {
            try
            {
                var result = await _employeeService.AddEmployee(dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(AddEmployee), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_EMPLOYEE_ERROR));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            try
            {
                var result = await _employeeService.UpdateEmployee(id, dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(UpdateEmployee), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployee(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(DeleteEmployee), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR));
            }
        }
    }
}
