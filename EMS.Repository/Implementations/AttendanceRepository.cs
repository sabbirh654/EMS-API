using Dapper;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public AttendanceRepository(
        IDatabaseFactory databaseFactory,
        IOperationLogRepository operationLogRepository,
        IDatabaseExceptionHandlerFactory databaseExceptionHandlerFactory)
    {
        _databaseFactory = databaseFactory;
        _operationLogRepository = operationLogRepository;
        _databaseExceptionHandlerFactory = databaseExceptionHandlerFactory;

        OnInit();
    }

    private void OnInit() => _exceptionHandler = _databaseExceptionHandlerFactory.GetHandler(DatabaseType.PostgreSql);

    public async Task<ApiResult> AddAsync(Attendance attendance)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("p_employee_id", attendance.EmployeeId, DbType.Int32);
                parameters.Add("p_attendance_date", attendance.Date, DbType.Date);
                parameters.Add("p_check_in_time", attendance.CheckInTime, DbType.Time);
                parameters.Add("p_check_out_time", attendance.CheckOutTime, DbType.Time);

                try
                {
                    await connection.ExecuteAsync("insert_attendance", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                    OperationLog log = new(OperationType.Add.ToString(), EntityName.Attendance.ToString(), "", $"New attendance has been added for employee id = {attendance.EmployeeId}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_ATTENDANCE_ERROR);
                }
            }
        }
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("p_id", id, DbType.Int32);

                try
                {
                    await connection.ExecuteAsync("delete_attendance", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                    OperationLog log = new(OperationType.Delete.ToString(), EntityName.Attendance.ToString(), $"{id}", $"Attendance has been deleted with Id = {id}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR);
                }
            }
        }
    }

    public Task<ApiResult> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResult> GetAllAsync(AttendanceFilter filter)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("p_employee_id", filter.EmployeeId, DbType.Int32);
            parameters.Add("p_date", filter.Date, DbType.Date);

            try
            {
                var result = await connection.QueryAsync<Attendance>("SELECT * FROM get_attendance(@p_employee_id, @p_date)", parameters);

                return ApiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR);
            }
        }
    }

    public async Task<ApiResult> GetAllByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("p_employee_id", id, DbType.Int32);

            try
            {
                var result = await connection.QueryAsync<AttendanceDetails>("SELECT * FROM get_attendance_history(@p_employee_id)", parameters);

                return ApiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR);
            }
        }
    }

    public Task<ApiResult> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResult> UpdateAsync(Attendance attendance)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("p_id", attendance.Id, DbType.Int32);
                parameters.Add("p_check_in_time", attendance.CheckInTime, DbType.Time);
                parameters.Add("p_check_out_time", attendance.CheckOutTime, DbType.Time);

                try
                {
                    await connection.ExecuteAsync("update_attendance", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                    OperationLog log = new(OperationType.Update.ToString(), EntityName.Attendance.ToString(), $"{attendance.Id}", $"Attendance has been updated for employee Id = {attendance.EmployeeId}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR);
                }
            }
        }
    }

    private bool CheckOverlappingInterval(List<Attendance> data, TimeSpan newCheckInTime, TimeSpan newCheckOutTime)
    {
        for (int i = 0; i < data?.Count; i++)
        {
            TimeSpan maxCheckInTime = data[i].CheckInTime > newCheckInTime ? data[i].CheckInTime : newCheckInTime;
            TimeSpan minCheckOutTime = data[i].CheckOutTime < newCheckOutTime ? data[i].CheckOutTime : newCheckOutTime;

            if (maxCheckInTime <= minCheckOutTime)
            {
                return false;
            }
        }

        return true;
    }
}
