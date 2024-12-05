namespace EMS.Core.Models;

public class ApiResult
{
    public bool IsSuccess { get; set; } = true;
    public int ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public dynamic? Result { get; set; }
}
