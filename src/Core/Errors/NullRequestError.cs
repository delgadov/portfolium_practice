namespace portfolium.Core.Errors;

public class NullRequestError()
    : ErrorResponse("The request is null", StatusCodes.Status400BadRequest);