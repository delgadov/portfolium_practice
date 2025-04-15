using portfolium.Application.Enums;

namespace portfolium.Application.DTOs;

public record StockUpdateRequestDto(string Symbol, string CompanyName, decimal CurrentPrice, IndustryType Industry, long MarketCap);