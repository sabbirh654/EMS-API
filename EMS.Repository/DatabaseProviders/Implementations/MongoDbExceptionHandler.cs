using DnsClient.Internal;
using EMS.Repository.DatabaseProviders.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EMS.Repository.DatabaseProviders.Implementations;

public class MongoDbExceptionHandler : IDatabaseExceptionHandler
{
    private readonly ILogger<MongoDbExceptionHandler> _logger;

    public MongoDbExceptionHandler(ILogger<MongoDbExceptionHandler> logger)
    {
        _logger = logger;
    }

    public void Handle(Exception ex)
    {
        if (ex is MongoException mongoEx)
        {
            _logger.LogError(ex.Message);

            #region specific error handling

            //switch (mongoEx.)
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
            _logger.LogError(ex, "Unexpected erro occurs in mongo db.");
        }
    }
}
