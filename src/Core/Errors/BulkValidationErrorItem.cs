namespace portfolium.Core.Errors;

public class BulkValidationErrorItem {
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public object AttemptedValue { get; set; }
}

// TODO TAKE A LOOK AT CHAT GPT, NEED TO IMPROVE THE RESPONSE USING THAT TEMPLATE