using ParkyWeb.Models.Interfaces;

namespace ParkyWeb.Models;

public class ApiResult<TResult> : IApiResult<TResult>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public TResult Result { get; set; }
}