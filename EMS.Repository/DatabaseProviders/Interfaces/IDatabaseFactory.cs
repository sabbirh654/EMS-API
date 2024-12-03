using MongoDB.Driver;
using System.Data;

namespace EMS.Repository.DatabaseProviders.Interfaces;

public interface IDatabaseFactory
{
    IDbConnection CreateSqlServerConnection();
    IDbConnection CreatePostgresSqlConnection();
    IMongoDatabase CreateMongoDbConnection();
}
