using EMS.API.Models;
using EMS.Core.Entities;
using EMS.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationLogsController : ControllerBase
    {
        private readonly IOperationLogRepository _operationLogRepository;

        public OperationLogsController(IOperationLogRepository operationLogRepository)
        {
            _operationLogRepository = operationLogRepository;
        }

        [HttpGet]
        public async Task<ApiResponse<List<OperationLog>>> GetLogsOfAEmployee([FromQuery] LogFilter filter)
        {
            ApiResponse<List<OperationLog>> apiResponse = new();

            try
            {
                var data = await _operationLogRepository.GetFilteredLogs(filter);
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
    }
}
