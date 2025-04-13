namespace portfolium.Core.Errors;

public record StockAlreadyExistError(string Message)
    : ErrorResponse($"Stock '{Message}' already exist", StatusCodes.Status409Conflict);