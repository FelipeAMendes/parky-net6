namespace ParkyWeb.Models.Interfaces;

public interface IApiResult<TResult>
{
    bool Success { get; set; }
    string Message { get; set; }
    TResult Result { get; set; }
}