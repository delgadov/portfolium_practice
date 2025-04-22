namespace portfolium.Core.Errors;

public class DatabaseError(string message)
    : ErrorResponse(message, StatusCodes.Status400BadRequest);