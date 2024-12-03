﻿using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class DesignationRepository : IDesignationRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<DesignationRepository> _logger;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public DesignationRepository(
        IDatabaseFactory databaseFactory, 
        ILogger<DesignationRepository>logger,
        IOperationLogRepository operationLogRepository,
        IDatabaseExceptionHandlerFactory databaseExceptionHandlerFactory,
        IDatabaseExceptionHandler databaseExceptionHandler)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
        _operationLogRepository = operationLogRepository;
        _databaseExceptionHandlerFactory = databaseExceptionHandlerFactory;

        OnInit();
    }

    private void OnInit() => _exceptionHandler = _databaseExceptionHandlerFactory.GetHandler(DatabaseType.SqlServer);

    public async Task AddAsync(Designation designation)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Name", designation.Name);

                try
                {
                    await connection.ExecuteAsync("AddNewDesignation", parameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);
                }
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Id", id);

                try
                {
                    await connection.ExecuteAsync("DeleteDesignation", parameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);
                }
            }
        }
    }

    public async Task<IEnumerable<Designation>?> GetAllAsync()
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            try
            {
                var result = await connection.QueryAsync<Designation>("GetAllDesignations", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
            }
        }
    }

    public async Task<Designation?> GetByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Id", id);

            try
            {
                var result = await connection.QuerySingleOrDefaultAsync<Designation>("GetDesignationById", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
            }
        }
    }

    public async Task UpdateAsync(Designation designation)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Id", designation.Id);
                parameters.Add("@Name", designation.Name);

                try
                {
                    await connection.ExecuteAsync("UpdateDesignation", parameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);
                }
            }
        }
    }
}
