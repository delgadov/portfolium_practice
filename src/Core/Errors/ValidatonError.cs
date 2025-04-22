namespace portfolium.Core.Errors;

public class ValidationError : ErrorResponse
{
    public List<string> Errors { get; init; }

    public ValidationError(List<string> errors) : base("Validation failed", StatusCodes.Status400BadRequest) {
        Errors = errors;
    }

    public ValidationError(string error) : base("Validation failed", StatusCodes.Status400BadRequest) {
        Errors = new List<string> { error };
    }
}