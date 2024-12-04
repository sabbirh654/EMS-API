using DnsClient.Internal;
using EMS.Repository.DatabaseProviders.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace EMS.Repository.DatabaseProviders.Implementations;

public class PostgreSqlExceptionHandler : IDatabaseExceptionHandler
{
    private readonly ILogger<PostgreSqlExceptionHandler> _logger;

    public PostgreSqlExceptionHandler(ILogger<PostgreSqlExceptionHandler> logger)
    {
        _logger = logger;
    }

    public void Handle(Exception ex)
    {
        if (ex is PostgresException postgreEx)
        {
            _logger.LogError(ex.Message);

            #region specific error handle

            //switch (postgreEx.SqlState)
            //{
            //    case "42883": //param error / sp not found error
            //        break;

            //    case "42P01": //table not found
            //        break;

            //    default:
            //        break;
            //}

            #endregion
        }
        else
        {
            _logger.LogError(ex, "Unexpected error occurs in postgresql db.");
        }
    }
}
