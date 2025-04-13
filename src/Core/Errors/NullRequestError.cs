namespace portfolium.Core.Errors;

public record NullRequestError()
    : ErrorResponse("The request is null", StatusCodes.Status400BadRequest);