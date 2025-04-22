namespace portfolium.Core.Errors;

public class StockAlreadyExistError(params string[] stock)
    : ErrorResponse(string.Join(", ", stock.Select(s => $"Stock '{s}' already exist")), StatusCodes.Status409Conflict);