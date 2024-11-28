using EMS.API.Models;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationsController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationsController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<Designation>>> GetAllDesignations()
        {
            ApiResponse<List<Designation>> apiResponse = new();

            try
            {
                var data = await _designationService.GetAllDesignations();
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
        public async Task<ApiResponse<Designation>> GetDesignationById(int id)
        {
            ApiResponse<Designation> apiResponse = new();

            try
            {
                var data = await _designationService.GetDesignationById(id);
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
        public async Task<ApiResponse<int>> AddDesignation([FromBody] AddUpdateDesignationDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _designationService.AddDesignation(dto);
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
        public async Task<ApiResponse<int>> UpdateDesignation(int id, [FromBody] AddUpdateDesignationDto dto)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _designationService.UpdateDesignation(id, dto);
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
        public async Task<ApiResponse<int>> DeleteDesignation(int id)
        {
            ApiResponse<int> apiResponse = new();

            try
            {
                await _designationService.DeleteDesignation(id);
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
