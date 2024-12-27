using Dapper;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class LoginRepository : ILoginRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public LoginRepository(
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

    public async Task<ApiResult> AddAsync(string username, string passwordHash)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Username", username);
                parameters.Add("@PasswordHash", passwordHash);

                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<User>("AddUser", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.REGISTER_USER_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> GetByUserNameAsync(string username)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Username", username);

                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<User>("GetUserLoginInfo", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_REGISTER_INFO_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> UpdateRefreshToken(int id, string newToken, DateTime newExpiryTime)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Id", id);
                parameters.Add("@NewToken", newToken);
                parameters.Add("@ExpiredAt", newExpiryTime);

                try
                {
                    var result = await connection.ExecuteAsync("UpdateRefreshToken", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_REGISTER_INFO_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> AddRefreshToken(int userId, string token, DateTime expiryTime)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@UserId", userId);
                parameters.Add("@RefreshToken", token);
                parameters.Add("@ExpiredAt", expiryTime);

                try
                {
                    var result = await connection.ExecuteAsync("AddRefreshToken", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_REGISTER_INFO_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> GetByRefreshToken(string refreshToken)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@RefreshToken", refreshToken);

                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<RefreshToken>("GetInfoByRefreshToken", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_REGISTER_INFO_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> GetUsernameByIdAsync(int userId)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Id", userId);

                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<string>("GetUsernameById", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_REGISTER_INFO_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> DeleteRefreshToken(string token)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("@Token", token);

                try
                {
                    var result = await connection.ExecuteAsync("DeleteRefreshToken", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_REGISTER_INFO_ERROR);
                }
            }
        }
    }
}
