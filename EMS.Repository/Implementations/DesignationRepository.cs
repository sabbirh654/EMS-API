using Dapper;
using EMS.Core.Entities;
using EMS.Repository.DatabaseProviders;
using EMS.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EMS.Repository.Implementations;

public class DesignationRepository : IDesignationRepository
{
    private readonly IDatabaseFactory _databaseFactory;
    private readonly ILogger<DesignationRepository> _logger;

    public DesignationRepository(IDatabaseFactory databaseFactory, ILogger<DesignationRepository>logger)
    {
        _databaseFactory = databaseFactory;
        _logger = logger;
    }

    public async Task AddAsync(Designation designation)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Name", designation.Name);

            try
            {
                await connection.ExecuteAsync("AddNewDesignation", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DesignationRepository)} at {AddAsync} function");
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
                await connection.ExecuteAsync("DeleteDesignation", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DesignationRepository)} at {DeleteAsync} function");
                throw;
            }
        }
    }

    public async Task<IEnumerable<Designation>?> GetAllAsync()
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            try
            {
                var result = await connection.QueryAsync<Designation>("GetAllDesignations", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DesignationRepository)} at {GetAllAsync} function");
                throw;
            }
        }
    }

    public async Task<Designation?> GetByIdAsync(int id)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();
            parameters.Add("@Id", id);

            try
            {
                var result = await connection.QuerySingleOrDefaultAsync<Designation>("GetDesignationById", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DesignationRepository)} at {GetByIdAsync} function");
                throw;
            }
        }
    }

    public async Task UpdateAsync(Designation designation)
    {
        using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
        {
            DynamicParameters parameters = new();

            parameters.Add("@Id", designation.Id);
            parameters.Add("@Name", designation.Name);

            try
            {
                await connection.ExecuteAsync("UpdateDesignation", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Database error in {nameof(DesignationRepository)} at {nameof(UpdateAsync)} function");
                throw;
            }
        }
    }
}
