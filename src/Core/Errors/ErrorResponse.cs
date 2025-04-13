namespace portfolium.Core.Errors;

public abstract record ErrorResponse(string Message, int StatusCode);