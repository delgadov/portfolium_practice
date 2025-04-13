using FluentValidation;
using portfolium.Application.DTOs;
using portfolium.Application.Enums;

namespace portfolium.Application.Validators;

public class StockRequestDtoValidator : AbstractValidator<StockRequestDto> {
    public StockRequestDtoValidator() {
        RuleFor(s => s.Symbol).NotEmpty()
                              .WithMessage("Symbol is required");

        RuleFor(s => s.CompanyName).NotEmpty()
                                   .WithMessage("CompanyName is required");

        RuleFor(s => s.CurrentPrice).GreaterThan(0)
                                    .WithMessage("CurrentPrice must be greater than 0");

        RuleFor(s => s.Industry).Must(type => Enum.IsDefined(typeof(IndustryType), type))
                                .WithMessage("Invalid IndustryType provided");

        RuleFor(s => s.MarketCap).GreaterThan(0)
                                 .WithMessage("MarketCap must be greater than 0");
    }
}