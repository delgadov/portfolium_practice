namespace portfolium.Core.Errors;

public class EmptyListError()
    : ErrorResponse("The list of Items are empty. Make sure it contains at least one.", StatusCodes.Status400BadRequest);