using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using MongoDB.Driver;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class OperationLogRepository : IOperationLogRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly IDatabaseExceptionHandlerFactory _handlerFactory;

    private IDatabaseExceptionHandler? _exceptionHandler;
    private IMongoDatabase? _mongoDatabase;
    private IMongoCollection<OperationLog>? _operationLogs;

    public OperationLogRepository(IDatabaseFactory databaseFactory, IDatabaseExceptionHandlerFactory handlerFactory)
    {
        _databaseFactory = databaseFactory;
        _handlerFactory = handlerFactory;

        OnInit();
    }

    private void OnInit()
    {
        _mongoDatabase = _databaseFactory.CreateMongoDbConnection();
        _operationLogs = _mongoDatabase.GetCollection<OperationLog>("OperationLogs");

        _exceptionHandler = _handlerFactory.GetHandler(DatabaseType.MongoDb);
    }

    public async Task<ApiResult> AddLogAsync(OperationLog log)
    {
        try
        {
            await _operationLogs.InsertOneAsync(log);

            return ApiResultFactory.CreateSuccessResult();

        }
        catch (Exception ex)
        {
            _exceptionHandler?.Handle(ex);

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_LOG_ERROR);
        }
    }

    //public async Task<IEnumerable<OperationLog>?> GetAllLogsAsync()
    //{
    //    try
    //    {
    //        return await _operationLogs.Find(log => true).ToListAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, $"Error while getting records from logs : {ex.Message}");
    //        throw;
    //    }
    //}

    public async Task<ApiResult> GetFilteredLogs(LogFilter f)
    {
        var filter = Builders<OperationLog>.Filter.And(
            Builders<OperationLog>.Filter.Eq(log => log.EntityId, f.id.ToString()),
            Builders<OperationLog>.Filter.Eq(log => log.EntityName, f.EntityName)
        );

        var sort = Builders<OperationLog>.Sort.Descending(log => log.Date)
                                          .Descending(log => log.Time);

        try
        {
            await _operationLogs.Find(filter).Sort(sort).ToListAsync();

            return ApiResultFactory.CreateSuccessResult();
        }
        catch (Exception ex)
        {
            _exceptionHandler?.Handle(ex);

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_LOG_ERROR);
        }
    }
}
