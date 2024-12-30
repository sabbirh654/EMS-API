using EMS.Core.DTOs;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IDepartmentService departmentService, ILogger<DepartmentsController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var result = await _departmentService.GetAllDepartments();

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentsController), nameof(GetAllDepartments), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR));
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Employee ID must be a positive integer."));
            }

            try
            {
                var result = await _departmentService.GetDepartmentById(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentsController), nameof(GetDepartmentById), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddDepartment([FromBody] AddUpdateDepartmentDto dto)
        {
            try
            {
                var result = await _departmentService.AddDepartment(dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentsController), nameof(AddDepartment), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_DEPARTMENT_ERROR));
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] AddUpdateDepartmentDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Department ID must be a positive integer."));
            }

            try
            {
                var result = await _departmentService.UpdateDepartment(id, dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentsController), nameof(UpdateDepartment), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DEPARTMENT_ERROR));
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Department ID must be a positive integer."));
            }

            try
            {
                var result = await _departmentService.DeleteDepartment(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DepartmentsController), nameof(DeleteDepartment), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DEPARTMENT_ERROR));
            }
        }
    }
}
