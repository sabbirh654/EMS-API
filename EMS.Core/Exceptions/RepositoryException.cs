namespace EMS.Core.Exceptions;

public class RepositoryException: CustomException
{
    public string ErrorCode { get; }

    public RepositoryException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
