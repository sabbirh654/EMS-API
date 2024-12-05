using Dapper;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public DepartmentRepository(
        IDatabaseFactory databaseFactory,
        IOperationLogRepository operationLogRepository,
        IDatabaseExceptionHandlerFactory databaseExceptionHandlerFactory)
    {
        _databaseFactory = databaseFactory;
        _operationLogRepository = operationLogRepository;
        _databaseExceptionHandlerFactory = databaseExceptionHandlerFactory;

        OnInit();
    }

    private void OnInit() => _exceptionHandler = _databaseExceptionHandlerFactory.GetHandler(DatabaseType.SqlServer);


    public async Task<ApiResult> AddAsync(Department department)
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

                    OperationLog log = new(OperationType.Add.ToString(), EntityName.Department.ToString(), "", $"New department has been added.");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_DEPARTMENT_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> DeleteAsync(int id)
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

                    OperationLog log = new(OperationType.Delete.ToString(), EntityName.Department.ToString(), $"{id}", $"Department has been deleted with Id = {id}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DEPARTMENT_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> GetAllAsync()
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            try
            {
                var result = await connection.QueryAsync<Department>("GetAllDepartments", commandType: CommandType.StoredProcedure);

                return ApiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
            }
        }
    }

    public async Task<ApiResult> GetByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Id", id);

            try
            {
                var result = await connection.QuerySingleOrDefaultAsync<Department>("GetDepartmentById", parameters, commandType: CommandType.StoredProcedure);

                return ApiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
            }
        }
    }

    public async Task<ApiResult> UpdateAsync(Department department)
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

                    OperationLog log = new(OperationType.Update.ToString(), EntityName.Department.ToString(), $"{department.Id}", $"Department has been updated with Id = {department.Id}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DEPARTMENT_ERROR);
                }
            }
        }
    }
}
