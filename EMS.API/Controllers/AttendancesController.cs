using EMS.API.Models;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<Attendance>>> GetAllAttendance([FromQuery] AttendanceFilter filter)
        {
            ApiResponse<List<Attendance>> apiResponse = new();

            try
            {
                var data = await _attendanceService.GetAllAttendance(filter);
                apiResponse.Success = true;
                apiResponse.Result = data?.ToList();
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }

        [HttpPost]
        public async Task<ApiResponse<int>> AddAttendance([FromBody] AddAttendanceDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _attendanceService.AddAttendance(dto);
                apiResponse.Success = true;
                apiResponse.Result = 1;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<int>> UpdateAttendance(int id, [FromBody] UpdateAttendanceDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _attendanceService.UpdateAttendance(id, dto);
                apiResponse.Success = true;
                apiResponse.Result = 1;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<int>> DeleteAttendance(int id)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _attendanceService.DeleteAttendance(id);
                apiResponse.Success = true;
                apiResponse.Result = 1;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }
    }
}
