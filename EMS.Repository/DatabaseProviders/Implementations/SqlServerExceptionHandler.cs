using DnsClient.Internal;
using EMS.Repository.DatabaseProviders.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EMS.Repository.DatabaseProviders.Implementations;

public class SqlServerExceptionHandler : IDatabaseExceptionHandler
{
    private readonly ILogger<SqlServerExceptionHandler> _logger;

    public SqlServerExceptionHandler(ILogger<SqlServerExceptionHandler> logger)
    {
        _logger = logger;
    }

    public void Handle(Exception ex)
    {
        if (ex is SqlException sqlEx)
        {
            _logger.LogError(ex.Message);

            #region specific error handling

            //switch (sqlEx.Number)
            //{
            //    case 201: //param error
            //        break;

            //    case 208: //table not found
            //        break;

            //    case 2812: //sp not found erro
            //        break;

            //    default:
            //        break;
            //}

            #endregion
        }
        else
        {
            _logger.LogError(ex, "Unexpected erro occurs.");
        }
    }
}
