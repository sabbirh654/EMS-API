﻿using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<DepartmentRepository> _logger;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public DepartmentRepository(
        IDatabaseFactory databaseFactory,
        ILogger<DepartmentRepository> logger,
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


    public async Task AddAsync(Department department)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Name", department.Name);

                try
                {
                    await connection.ExecuteAsync("AddNewDepartment", parameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
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
                    await connection.ExecuteAsync("DeleteDepartment", parameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _exceptionHandler?.Handle(ex);
                }
            }
        }
    }

    public async Task<IEnumerable<Department>?> GetAllAsync()
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            try
            {
                var result = await connection.QueryAsync<Department>("GetAllDepartments", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
            }
        }
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Id", id);

            try
            {
                var result = await connection.QuerySingleOrDefaultAsync<Department>("GetDepartmentById", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
            }
        }
    }

    public async Task UpdateAsync(Department department)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Id", department.Id);
                parameters.Add("@Name", department.Name);

                try
                {
                    await connection.ExecuteAsync("UpdateDepartment", parameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _exceptionHandler?.Handle(ex);
                }
            }
        }
    }
}
