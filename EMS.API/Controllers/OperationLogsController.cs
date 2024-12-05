using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationLogsController : ControllerBase
    {
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly ILogger<OperationLogsController> _logger;

        public OperationLogsController(IOperationLogRepository operationLogRepository, ILogger<OperationLogsController> logger)
        {
            _operationLogRepository = operationLogRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeLogs([FromQuery] LogFilter filter)
        {
            try
            {
                var result = await _operationLogRepository.GetFilteredLogs(filter);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(OperationLogsController), nameof(GetEmployeeLogs), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_LOG_ERROR));
            }
        }
    }
}
