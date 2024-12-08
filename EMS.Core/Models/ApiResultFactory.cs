namespace EMS.Core.Models;

public static class ApiResultFactory
{
    public static ApiResult CreateErrorResult(int errorCode, string errorMessage, List<string> errors = null)
    {
        return new ApiResult
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage,
            Errors = errors
        };
    }

    public static ApiResult CreateSuccessResult(dynamic? result = null)
    {
        return new ApiResult
        {
            IsSuccess = true,
            Result = result
        };
    }
}
