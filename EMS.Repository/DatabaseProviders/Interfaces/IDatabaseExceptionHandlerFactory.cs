using static EMS.Core.Enums;

namespace EMS.Repository.DatabaseProviders.Interfaces;

public interface IDatabaseExceptionHandlerFactory
{
    IDatabaseExceptionHandler GetHandler(DatabaseType type);
}
