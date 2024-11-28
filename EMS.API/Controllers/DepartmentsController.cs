using EMS.API.Models;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<Department>>> GetAllDepartments()
        {
            ApiResponse<List<Department>> apiResponse = new();

            try
            {
                var data = await _departmentService.GetAllDepartments();
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

        [HttpGet("{id}")]
        public async Task<ApiResponse<Department>> GetDepartmentById(int id)
        {
            ApiResponse<Department> apiResponse = new();

            try
            {
                var data = await _departmentService.GetDepartmentById(id);
                apiResponse.Success = true;
                apiResponse.Result = data;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }

        [HttpPost]
        public async Task<ApiResponse<int>> AddDepartment([FromBody] AddUpdateDepartmentDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _departmentService.AddDepartment(dto);
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
        public async Task<ApiResponse<int>> UpdateDepartment(int id, [FromBody] AddUpdateDepartmentDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _departmentService.UpdateDepartment(id, dto);
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
        public async Task<ApiResponse<int>> DeleteDepartment(int id)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _departmentService.DeleteDepartment(id);
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
