using Dapper;
using DnsClient.Internal;
using EMS.Core.Entities;
using EMS.Core.Exceptions;
using EMS.Repository.DatabaseProviders;
using EMS.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace EMS.Repository.Implementations;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<AttendanceRepository> _logger;

    public AttendanceRepository(IDatabaseFactory databaseFactory, ILogger<AttendanceRepository> logger)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
    }

    public async Task AddAsync(Attendance attendance)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("p_employee_id", attendance.EmployeeId, DbType.Int32);
            parameters.Add("p_attendance_date", attendance.Date, DbType.Date);
            parameters.Add("p_check_in_time", attendance.CheckInTime, DbType.Time);
            parameters.Add("p_check_out_time", attendance.CheckOutTime, DbType.Time);

            try
            {
                await connection.ExecuteAsync("insert_attendance", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "P0001")
                {
                    _logger.LogError(ex, $"Database error in {nameof(AttendanceRepository)} at {nameof(AddAsync)} function");
                    throw new RepositoryException(message: ex.Message, errorCode: ex.SqlState);
                }
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("p_id", id, DbType.Int32);

            try
            {
                await connection.ExecuteAsync("delete_attendance", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(AttendanceRepository)} at {nameof(DeleteAsync)} function");
                throw;
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
            catch (PostgresException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(AttendanceRepository)} at {nameof(GetAllAsync)} function");
                throw;
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
            DynamicParameters parameters = new();

            parameters.Add("p_id", attendance.Id, DbType.Int32);
            parameters.Add("p_check_in_time", attendance.CheckInTime, DbType.Time);
            parameters.Add("p_check_out_time", attendance.CheckOutTime, DbType.Time);

            try
            {
                await connection.ExecuteAsync("update_attendance", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "P0001")
                {
                    _logger.LogError(ex, $"Database error in {nameof(AttendanceRepository)} at {nameof(AddAsync)} function");
                    throw new RepositoryException(message: ex.Message, errorCode: ex.SqlState);
                }
            }
        }
    }
}
