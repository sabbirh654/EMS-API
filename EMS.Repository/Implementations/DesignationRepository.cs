using Dapper;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class DesignationRepository : IDesignationRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public DesignationRepository(
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

    public async Task<ApiResult> AddAsync(Designation designation)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Name", designation.Name);

                try
                {
                    await connection.ExecuteAsync("AddNewDesignation", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                    OperationLog log = new(OperationType.Add.ToString(), EntityName.Designation.ToString(), "", $"New designation has been added.");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_DESIGNATION_ERROR);
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
                    await connection.ExecuteAsync("DeleteDesignation", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                    OperationLog log = new(OperationType.Delete.ToString(), EntityName.Designation.ToString(), $"{id}", $"Designation has been deleted with Id = {id}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DESIGNATION_ERROR);
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
                var result = await connection.QueryAsync<Designation>("GetAllDesignations", commandType: CommandType.StoredProcedure);

                return ApiResultFactory.CreateSuccessResult(result);

            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
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
                var result = await connection.QuerySingleOrDefaultAsync<Designation>("GetDesignationById", parameters, commandType: CommandType.StoredProcedure);

                return ApiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
            }
        }
    }

    public async Task<ApiResult> UpdateAsync(Designation designation)
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
                    await connection.ExecuteAsync("UpdateDesignation", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                    OperationLog log = new(OperationType.Update.ToString(), EntityName.Designation.ToString(), $"{designation.Id}", $"Designation has been updated with Id = {designation.Id}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DESIGNATION_ERROR);
                }
            }
        }
    }
}
