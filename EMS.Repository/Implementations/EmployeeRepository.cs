using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders;
using EMS.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EMS.Repository.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(IDatabaseFactory databaseFactory, ILogger<EmployeeRepository> logger)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
    }

    public async Task AddAsync(Employee employee)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Name", employee.Name);
            parameters.Add("@Email", employee.Email);
            parameters.Add("@Phone", employee.PhoneNumber);
            parameters.Add("@Address", employee.Address);
            parameters.Add("@DOB", employee.BirthDate);
            parameters.Add("@DesignationId", employee.DesignationId);
            parameters.Add("@DepartmentId", employee.DepartmentId);

            try
            {
                await connection.ExecuteAsync("AddNewEmployee", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(EmployeeRepository)} at {AddAsync} function");
                throw;
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();
            parameters.Add("@Id", id);

            try
            {
                await connection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(EmployeeRepository)} at {DeleteAsync} function");
                throw;
            }
        }
    }

    public async Task<IEnumerable<EmployeeDetails>?> GetAllAsync()
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            try
            {
                var result = await connection.QueryAsync<EmployeeDetails>("GetAllEmployees", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(EmployeeRepository)} at {GetAllAsync} function");
                throw;
            }
        }
    }

    public async Task<EmployeeDetails?> GetByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();
            parameters.Add("@Id", id);

            try
            {
                var result = await connection.QuerySingleOrDefaultAsync<EmployeeDetails>("GetEmployeeById", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(EmployeeRepository)} at {GetByIdAsync} function");
                throw;
            }
        }
    }

    public async Task UpdateAsync(Employee employee)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Id", employee.Id);
            parameters.Add("@Name", employee.Name);
            parameters.Add("@Email", employee.Email);
            parameters.Add("@Phone", employee.PhoneNumber);
            parameters.Add("@DOB", employee.BirthDate);
            parameters.Add("@Address", employee.Address);

            try
            {
                await connection.ExecuteAsync("UpdateEmployee", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(EmployeeRepository)} at {nameof(UpdateAsync)} function");
                throw;
            }
        }
    }
}
