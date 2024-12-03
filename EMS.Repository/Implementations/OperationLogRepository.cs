﻿using DnsClient.Internal;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EMS.Repository.Implementations;

public class OperationLogRepository : IOperationLogRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<OperationLog> _operationLogs;
    private readonly ILogger<OperationLogRepository> _logger;

    public OperationLogRepository(IDatabaseFactory databaseFactory, ILogger<OperationLogRepository> logger)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
        _mongoDatabase = _databaseFactory.CreateMongoDbConnection();
        _operationLogs = _mongoDatabase.GetCollection<OperationLog>("OperationLogs");
    }

    public async Task AddLogAsync(OperationLog log)
    {
        try
        {
            await _operationLogs.InsertOneAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while adding a record in the log with {ex.Message}");
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

        var result =  await _operationLogs.Find(filter).Sort(sort).ToListAsync();
        return result;
    }
}
