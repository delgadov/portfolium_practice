namespace portfolium.Core.Errors;

public class ValidationFailures {
    public int index { get; set; }
    public string StockSymbol { get; set; }
    public List<BulkValidationErrorItem> FailedItems { get; set; } = [];
}