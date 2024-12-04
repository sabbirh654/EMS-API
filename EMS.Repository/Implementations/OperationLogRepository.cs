using EMS.Core.Entities;
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

    public async Task AddLogAsync(OperationLog log)
    {
        try
        {
            await _operationLogs.InsertOneAsync(log);
        }
        catch (Exception ex)
        {
            _exceptionHandler?.Handle(ex);
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

    public async Task<IEnumerable<OperationLog>?> GetFilteredLogs(LogFilter f)
    {
        var filter = Builders<OperationLog>.Filter.And(
            Builders<OperationLog>.Filter.Eq(log => log.EntityId, f.id.ToString()),
            Builders<OperationLog>.Filter.Eq(log => log.EntityName, f.EntityName)
        );

        var sort = Builders<OperationLog>.Sort.Descending(log => log.Date)
                                          .Descending(log => log.Time);

        try
        {
            return await _operationLogs.Find(filter).Sort(sort).ToListAsync();
        }
        catch (Exception ex)
        {
            _exceptionHandler?.Handle(ex);
            return null;
        }
    }
}
