using portfolium.Application.Enums;

namespace portfolium.Application.DTOs;

public record StockRequestDto(
    string Symbol,
    string CompanyName,
    decimal CurrentPrice,
    IndustryType Industry,
    long MarketCap);