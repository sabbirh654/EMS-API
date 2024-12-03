using EMS.Repository.DatabaseProviders.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Npgsql;
using System.Data;

namespace EMS.Repository.DatabaseProviders.Implementations;

public class DatabaseFactory : IDatabaseFactory
{
    private readonly IConfiguration _configuration;

    public DatabaseFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateSqlServerConnection()
    {
        var connection = new SqlConnection(_configuration.GetConnectionString("SqlServerConnection"));
        try
        {
            connection.Open();
            return connection;
        }
        catch (SqlException ex)
        {
            connection.Dispose();
            throw new InvalidOperationException("Failed to establish SqlServer connection", ex);
        }
    }

    public IDbConnection CreatePostgresSqlConnection()
    {
        var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSqlConnection"));
        try
        {
            connection.Open();
            return connection;
        }
        catch (NpgsqlException ex)
        {
            connection.Dispose();
            throw new InvalidOperationException("Failed to establish PostgreSql connection", ex);
        }
    }

    public IMongoDatabase CreateMongoDbConnection()
    {
        try
        {
            var client = new MongoClient(_configuration.GetConnectionString("MongoDbConnection"));
            return client.GetDatabase(_configuration["ConnectionStrings:MongoDbDatabaseName"]);
        }
        catch (MongoException ex)
        {
            throw new InvalidOperationException("Failed to obtain MongoDB database", ex);
        }
    }
}