namespace EMS.Repository.DatabaseProviders.Interfaces;

public interface IDatabaseExceptionHandler
{
    void Handle(Exception exception);
}
