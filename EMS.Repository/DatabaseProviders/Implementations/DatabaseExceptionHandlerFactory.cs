using EMS.Repository.DatabaseProviders.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using static EMS.Core.Enums;

namespace EMS.Repository.DatabaseProviders.Implementations;

public class DatabaseExceptionHandlerFactory : IDatabaseExceptionHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseExceptionHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDatabaseExceptionHandler GetHandler(DatabaseType type)
    {
        return type switch
        {
            DatabaseType.SqlServer => _serviceProvider.GetRequiredService<SqlServerExceptionHandler>(),
            DatabaseType.PostgreSql => _serviceProvider.GetRequiredService<PostgreSqlExceptionHandler>(),
            DatabaseType.MongoDb => _serviceProvider.GetRequiredService<MongoDbExceptionHandler>(),
            _ => throw new NotImplementedException($"Database type {type.ToString()} is not supported.")
        };
    }
}
