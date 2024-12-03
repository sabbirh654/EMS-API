using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<EmployeeRepository> _logger;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public EmployeeRepository(
        IDatabaseFactory databaseFactory,
        ILogger<EmployeeRepository> logger,
        IOperationLogRepository operationLogRepository,
        IDatabaseExceptionHandlerFactory databaseExceptionHandlerFactory)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
        _operationLogRepository = operationLogRepository;
        _databaseExceptionHandlerFactory = databaseExceptionHandlerFactory;

        OnInit();
    }

    private void OnInit() => _exceptionHandler = _databaseExceptionHandlerFactory.GetHandler(DatabaseType.SqlServer);

    public async Task AddAsync(Employee employee)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
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
                    await connection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);

                    OperationLog log = new(OperationType.DELETE.ToString(), EntityName.Employee.ToString(), $"{id}", $"Employee has been added with Id = {id}");
                    await _operationLogRepository.AddLogAsync(log);

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

    public async Task<IEnumerable<EmployeeDetails>?> GetAllAsync()
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            try
            {
                var result = await connection.QueryAsync<EmployeeDetails>("GetAllEmployees", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
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
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);
                return null;
            }
        }
    }

    public async Task UpdateAsync(Employee employee)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            using (var transaction = connection.BeginTransaction())
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
}
