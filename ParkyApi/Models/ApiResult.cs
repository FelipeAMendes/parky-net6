using ParkyApi.Models.Interfaces;

namespace ParkyApi.Models;

public class ApiResult : IApiResult
{
    public ApiResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public ApiResult(bool success, string message, object result)
    {
        Success = success;
        Message = message;
        Result = result;
    }

    public ApiResult(bool success, object result)
    {
        Success = success;
        Result = result;
    }

    public ApiResult(bool success)
    {
        Success = success;
    }

    public ApiResult(object result)
    {
        Success = true;
        Result = result;
    }

    public bool Success { get; set; }
    public string Message { get; set; }
    public object Result { get; set; }
}