using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders;
using EMS.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EMS.Repository.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(IDatabaseFactory databaseFactory, ILogger<DepartmentRepository> logger)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
    }

    public async Task AddAsync(Department department)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Name", department.Name);

            try
            {
                await connection.ExecuteAsync("AddNewDepartment", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DepartmentRepository)} at {AddAsync} function");
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
                await connection.ExecuteAsync("DeleteDepartment", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DepartmentRepository)} at {DeleteAsync} function");
                throw;
            }
        }
    }

    public async Task<IEnumerable<Department>?> GetAllAsync()
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            try
            {
                var result = await connection.QueryAsync<Department>("GetAllDepartments", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DepartmentRepository)} at {GetAllAsync} function");
                throw;
            }
        }
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();
            parameters.Add("@Id", id);

            try
            {
                var result = await connection.QuerySingleOrDefaultAsync<Department>("GetDepartmentById", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DepartmentRepository)} at {GetByIdAsync} function");
                throw;
            }
        }
    }

    public async Task UpdateAsync(Department department)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Id", department.Id);
            parameters.Add("@Name", department.Name);

            try
            {
                await connection.ExecuteAsync("UpdateDepartment", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DepartmentRepository)} at {nameof(UpdateAsync)} function");
                throw;
            }
        }
    }
}
