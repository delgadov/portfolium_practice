using FluentValidation;
using portfolium.Application.DTOs;
using portfolium.Application.Enums;

namespace portfolium.Application.Validators;

public class StockUpdateRequestDtoValidator : AbstractValidator<StockUpdateRequestDto> {
    public StockUpdateRequestDtoValidator() {
        RuleFor(s => s.Symbol)
            .MaximumLength(5).WithMessage("Symbol cannot exceed 5 characters");

        RuleFor(s => s.CompanyName)
            .MaximumLength(30).WithMessage("CompanyName cannot exceed 30 characters");

        RuleFor(s => s.CurrentPrice)
            .GreaterThan(0)
            .WithMessage("CurrentPrice must be greater than 0")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("CurrentPrice exceeds maximum allowed value");

        RuleFor(s => s.Industry)
            .Must(type => !type.HasValue || Enum.IsDefined(typeof(IndustryType), type.Value))
            .WithMessage("Invalid IndustryType provided");

        RuleFor(s => s.MarketCap)
            .GreaterThan(0).WithMessage("MarketCap must be greater than 0");
    }
}