using MongoDB.Driver;
using System.Data;

namespace EMS.Repository.DatabaseProviders;

public interface IDatabaseFactory
{
    IDbConnection CreateSqlServerConnection();
    IDbConnection CreatePostgresSqlConnection();
    IMongoDatabase CreateMongoDbConnection();
}
