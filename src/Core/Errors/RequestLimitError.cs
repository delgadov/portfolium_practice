namespace portfolium.Core.Errors;

public class RequestLimitError(int threshold)
    : ErrorResponse($"You are exceeding the amount of '{threshold}' per request", StatusCodes.Status400BadRequest);