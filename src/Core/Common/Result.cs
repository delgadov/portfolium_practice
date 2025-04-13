using System.Text.Json.Serialization;
using portfolium.Core.Errors;

namespace portfolium.Core.Common;

public class Result<T> {
    private Result(T data) {
        Data = data;
        IsSuccess = true;
    }

    private Result(ErrorResponse errorResponse) {
        ErrorResponse = errorResponse;
        IsSuccess = false;
    }

    public T Data { get; set; }
    [JsonIgnore]
    public ErrorResponse ErrorResponse { get; set; }
    public bool IsSuccess { get; set; }

    public static Result<T> Success(T data) {
        return new Result<T>(data);
    }

    public static Result<T> Fail(ErrorResponse errorResponse) {
        return new Result<T>(errorResponse);
    }
}