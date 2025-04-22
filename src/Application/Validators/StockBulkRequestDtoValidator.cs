using portfolium.Application.DTOs;
using portfolium.Core.Errors;

namespace portfolium.Application.Validators;

public class StockBulkRequestDtoValidator {
    public List<ValidationFailures> Validate(List<StockRequestDto> stockRequestDtos) {
        var validationFailuresList = new List<ValidationFailures>();
        var validator = new StockRequestDtoValidator();

        for (var i = 0; i < stockRequestDtos.Count; i++) {
            var validationResult = validator.Validate(stockRequestDtos[i]);

            if (!validationResult.IsValid)
                validationFailuresList.Add(new ValidationFailures {
                    index = i,
                    StockSymbol = stockRequestDtos[i].Symbol,
                    FailedItems = validationResult.Errors
                                            .Select(x => new BulkValidationErrorItem {
                                                PropertyName = x.PropertyName,
                                                ErrorMessage = x.ErrorMessage,
                                                AttemptedValue = x.AttemptedValue
                                            })
                                            .ToList()
                });
        }

        return validationFailuresList;
    }
}