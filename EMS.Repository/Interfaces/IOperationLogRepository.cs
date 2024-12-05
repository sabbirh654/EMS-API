using EMS.Core.Entities;
using EMS.Core.Models;

namespace EMS.Repository.Interfaces;

public interface IOperationLogRepository
{
    Task<ApiResult> AddLogAsync(OperationLog log);
    Task<ApiResult> GetFilteredLogs(LogFilter filter);
}
