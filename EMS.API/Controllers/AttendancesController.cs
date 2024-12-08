using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<AttendancesController> _logger;

        public AttendancesController(IAttendanceService attendanceService, ILogger<AttendancesController> logger)
        {
            _attendanceService = attendanceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttendance([FromQuery] AttendanceFilter filter)
        {
            try
            {
                var result = await _attendanceService.GetAllAttendance(filter);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendancesController), nameof(GetAllAttendance), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR));
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetSingleEmployeeAttendance(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Employee ID must be a positive integer."));
            }

            try
            {
                var result = await _attendanceService.GetEmployeeAttendance(employeeId);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendancesController), nameof(GetSingleEmployeeAttendance), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAttendance([FromBody] AddAttendanceDto dto)
        {
            try
            {
                var result = await _attendanceService.AddAttendance(dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendancesController), nameof(AddAttendance), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_ATTENDANCE_ERROR));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] UpdateAttendanceDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Attendance ID must be a positive integer."));
            }

            try
            {
                var result = await _attendanceService.UpdateAttendance(id, dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendancesController), nameof(UpdateAttendance), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Attendance ID must be a positive integer."));
            }

            try
            {
                var result = await _attendanceService.DeleteAttendance(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendancesController), nameof(DeleteAttendance), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR));
            }
        }
    }
}
