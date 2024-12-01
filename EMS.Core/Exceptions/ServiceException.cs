namespace EMS.Core.Exceptions;

public class ServiceException : CustomException
{
    public string ErrorCode { get; }

    public ServiceException(string message, string errorCode) : base(message) 
    {
        ErrorCode = errorCode;
    }
}
