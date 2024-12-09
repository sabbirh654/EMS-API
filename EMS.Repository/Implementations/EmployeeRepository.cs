using Dapper;
using EMS.Core.ChangeTrackers;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Interfaces;
using System.Data;
using System.Linq;
using static EMS.Core.Enums;

namespace EMS.Repository.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly IDatabaseExceptionHandlerFactory _databaseExceptionHandlerFactory;
    private IDatabaseExceptionHandler? _exceptionHandler;

    public EmployeeRepository(
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

    public async Task<ApiResult> AddAsync(Employee employee)
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
                    int newId = await connection.ExecuteAsync("AddNewEmployee", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    int employeeId = parameters.Get<int>("@Id");

                    OperationLog log = new(OperationType.Add.ToString(), EntityName.Employee.ToString(), $"{employeeId}", $"Employee has been added with Id = {employeeId}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_EMPLOYEE_ERROR);
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
                    await connection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                    OperationLog log = new(OperationType.Delete.ToString(), EntityName.Employee.ToString(), $"{id}", $"Employee has been added with Id = {id}");
                    await _operationLogRepository.AddLogAsync(log);

                    transaction.Commit();

                    return ApiResultFactory.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _exceptionHandler?.Handle(ex);

                    return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR);
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
                var result = await connection.QueryAsync<EmployeeDetails>("GetAllEmployees", commandType: CommandType.StoredProcedure);

                return ApiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR);
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
                var result = await connection.QuerySingleOrDefaultAsync<EmployeeDetails>("GetEmployeeById", parameters, commandType: CommandType.StoredProcedure);

                return ApiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Handle(ex);

                return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR);
            }
        }
    }

    public async Task<ApiResult> UpdateAsync(Employee employee)
    {
        using var connection = _databaseFactory.CreateSqlServerConnection();
        using var transaction = connection.BeginTransaction();


        DynamicParameters parameters = new();

        parameters.Add("@Id", employee.Id);
        parameters.Add("@Name", employee.Name);
        parameters.Add("@Email", employee.Email);
        parameters.Add("@Phone", employee.PhoneNumber);
        parameters.Add("@DOB", employee.BirthDate);
        parameters.Add("@Address", employee.Address);



        try
        {
            var existingEmployee = await GetByIdAsync(employee.Id);
            var updatedFields = EmployeeChangeTracker.GetUpdatedFields(existingEmployee.Result, employee);

            if (updatedFields.Count == 0)
            {
            }

            await connection.ExecuteAsync("UpdateEmployee", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

            string changes = string.Join(Environment.NewLine, updatedFields);
            OperationLog log = new(OperationType.Update.ToString(), EntityName.Employee.ToString(), $"{employee.Id}", $"Employee updated\n. Changes:\n {changes}");
            await _operationLogRepository.AddLogAsync(log);

            transaction.Commit();

            return ApiResultFactory.CreateSuccessResult();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _exceptionHandler?.Handle(ex);

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR);
        }
    }
}
