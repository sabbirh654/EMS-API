using EMS.API.Models;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<EmployeeDetails>>> GetAllEmployees()
        {
            ApiResponse<List<EmployeeDetails>> apiResponse = new();

            try
            {
                var data = await _employeeService.GetAllEmployees();
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
        public async Task<ApiResponse<EmployeeDetails>> GetEmployeeById(int id)
        {
            ApiResponse<EmployeeDetails> apiResponse = new();

            try
            {
                var data = await _employeeService.GetEmployeeById(id);
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
        public async Task<ApiResponse<int>> AddEmployee(AddEmployeeDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _employeeService.AddEmployee(dto);
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
        public async Task<ApiResponse<int>> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _employeeService.UpdateEmployee(id, dto);
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
        public async Task<ApiResponse<int>> DeleteEmployee(int id)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _employeeService.DeleteEmployee(id);
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
