using FluentValidation;
using portfolium.Web.Filters;

namespace portfolium.Application.Validators;

public class StockFilterRequestValidator : AbstractValidator<StockFilterRequest> {
    public StockFilterRequestValidator() {
        RuleFor(filter => filter.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal to 1")
            .LessThanOrEqualTo(100).WithMessage("PageSize must be less than or equal to 100");

        RuleFor(filter => filter.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1");

        RuleFor(filter => filter.MinCurrentPrice)
            .GreaterThan(0).WithMessage("Min current price must be greater than 0");

        RuleFor(filter => filter.MaxCurrentPrice)
            .GreaterThan(0).WithMessage("Max current price must be greater than 0");

        RuleFor(filter => filter.MinMarketCap)
            .GreaterThan(0).WithMessage("Min Market Cap must be greater than 0");

        RuleFor(filter => filter.MaxMarketCap)
            .GreaterThan(0).WithMessage("Max Market Cap must be greater than 0");

        RuleFor(filter => filter.MaxCurrentPrice)
            .GreaterThanOrEqualTo(filter => filter.MinCurrentPrice ?? 0)
            .When(filter => filter.MinCurrentPrice.HasValue && filter.MaxCurrentPrice.HasValue)
            .WithMessage("Max Current Price must be greater than or equal to Min Current Price");

        RuleFor(filter => filter.MaxMarketCap)
            .GreaterThanOrEqualTo(filter => filter.MinMarketCap ?? 0)
            .When(filter => filter.MinMarketCap.HasValue && filter.MaxMarketCap.HasValue)
            .WithMessage("Max Market Cap must be greater than or equal to Min Market Cap");

        RuleFor(filter => filter.Symbol)
            .MaximumLength(10)
            .Matches("^[A-Z0-9.]*$")
            .When(filter => filter.Symbol != null)
            .WithMessage("Symbol must contain only uppercase letters, numbers, and dots");

        RuleFor(filter => filter.CompanyName)
            .MaximumLength(100)
            .When(filter => filter.CompanyName != null)
            .WithMessage("Company name cannot exceed 100 characters");

        RuleFor(filter => filter.Industry)
            .IsInEnum()
            .When(filter => filter.Industry.HasValue)
            .WithMessage("Invalid industry type");
    }
}