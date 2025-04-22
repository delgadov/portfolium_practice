using Newtonsoft.Json;
using portfolium.Core.Errors;

namespace portfolium.Core.Common;

public class Result<T> {
    private Result(T data, string message) {
        Data = data;
        IsSuccess = true;
        Message = message;
    }

    private Result(ErrorResponse errorResponse) {
        ErrorResponse = errorResponse;
        IsSuccess = false;
    }

    [JsonIgnore] public ErrorResponse ErrorResponse { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static Result<T> Success(T data, string message = "Success") {
        return new Result<T>(data, message);
    }

    public static Result<T> Fail(ErrorResponse errorResponse) {
        return new Result<T>(errorResponse);
    }
}