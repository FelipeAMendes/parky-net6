namespace ParkyApi.Models.Interfaces;

public interface IApiResult
{
    bool Success { get; set; }
    string Message { get; set; }
    object Result { get; set; }
}