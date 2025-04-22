namespace portfolium.Core.Errors;

public abstract class ErrorResponse(string Message, int StatusCode) {
    public string Message { get; init; } = Message;
    public int StatusCode { get; init; } = StatusCode;

    public void Deconstruct(out string Message, out int StatusCode) {
        Message = this.Message;
        StatusCode = this.StatusCode;
    }
}