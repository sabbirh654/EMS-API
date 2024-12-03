using EMS.Core.Entities;

namespace EMS.Repository.Interfaces;

public interface IOperationLogRepository
{
    Task AddLogAsync(OperationLog log);
    Task<IEnumerable<OperationLog>?> GetFilteredLogs(LogFilter filter);
}
