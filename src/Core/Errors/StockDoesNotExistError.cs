namespace portfolium.Core.Errors;

public class StockDoesNotExistError(Guid Id)
    : ErrorResponse($"The Stock {Id.ToString()} does not exist", StatusCodes.Status404NotFound);