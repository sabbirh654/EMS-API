using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<AttendanceRepository> _logger;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public AttendanceRepository(
        IDatabaseFactory databaseFactory,
        ILogger<AttendanceRepository> logger,
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

    private void OnInit() => _exceptionHandler = _databaseExceptionHandlerFactory.GetHandler(DatabaseType.PostgreSql);

    public async Task AddAsync(Attendance attendance)
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
                    await connection.ExecuteAsync("insert_attendance", parameters, commandType: CommandType.StoredProcedure);
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
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                DynamicParameters parameters = new();

                parameters.Add("p_id", id, DbType.Int32);

                try
                {
                    await connection.ExecuteAsync("delete_attendance", parameters, commandType: CommandType.StoredProcedure);
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

    public Task<IEnumerable<Attendance>?> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Attendance>?> GetAllAsync(AttendanceFilter filter)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("p_employee_id", filter.EmployeeId, DbType.Int32);
            parameters.Add("p_date", filter.Date, DbType.Date);

            try
            {
                var result = await connection.QueryAsync<Attendance>("SELECT * FROM get_attendance(@p_employee_id, @p_date)", parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
            }
        }
    }

    public async Task<IEnumerable<AttendanceDetails>?> GetAllByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("p_employee_id", id, DbType.Int32);

            try
            {
                var result = await connection.QueryAsync<AttendanceDetails>("SELECT * FROM get_attendance_history(@p_employee_id)", parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
            }
        }
    }

    public Task<Attendance?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Attendance attendance)
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
                    await connection.ExecuteAsync("update_attendance", parameters, commandType: CommandType.StoredProcedure);
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
