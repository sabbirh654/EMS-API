using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders;
using EMS.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<EmployeeRepository> _logger;
    private readonly IOperationLogRepository _operationLogRepository;

    public EmployeeRepository(IDatabaseFactory databaseFactory, ILogger<EmployeeRepository> logger, IOperationLogRepository operationLogRepository)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
        _operationLogRepository = operationLogRepository;
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
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                int newId = await connection.ExecuteAsync("AddNewEmployee", parameters, commandType: CommandType.StoredProcedure);
                int employeeId = parameters.Get<int>("@Id");

                OperationLog log = new(OperationType.ADD.ToString(), EntityName.Employee.ToString(), $"{employeeId}", $"Employee has been added with Id = {employeeId}");
                await _operationLogRepository.AddLogAsync(log);
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

                OperationLog log = new(OperationType.DELETE.ToString(), EntityName.Employee.ToString(), $"{id}", $"Employee has been added with Id = {id}");
                await _operationLogRepository.AddLogAsync(log);
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

                OperationLog log = new(OperationType.UPDATE.ToString(), EntityName.Employee.ToString(), $"{employee.Id}", $"Employee has been added with Id = {employee.Id}");
                await _operationLogRepository.AddLogAsync(log);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(EmployeeRepository)} at {nameof(UpdateAsync)} function");
                throw;
            }
        }
    }
}
