using Microsoft.IdentityModel.Tokens;
using portfolium.Application.DTOs;
using portfolium.Application.Interfaces;
using portfolium.Core.Entities;

namespace portfolium.Application.Mappers;

public class StockMapper : IStockMapper {
    public StockResponseDto FromStock(Stock stock) {
        if (stock == null) throw new ArgumentNullException(nameof(stock));

        return new StockResponseDto(
            stock.StockId,
            stock.Symbol,
            stock.CompanyName,
            stock.CurrentPrice,
            stock.Industry,
            stock.MarketCap
        );
    }

    public List<StockResponseDto> FromStocks(List<Stock> stocks) {
        return stocks.Select(FromStock).ToList();
    }

    public Stock FromRequestToStock(StockRequestDto stockRequest) {
        if (stockRequest == null) throw new ArgumentNullException(nameof(stockRequest));

        return new Stock {
            Symbol = stockRequest.Symbol,
            CompanyName = stockRequest.CompanyName,
            CurrentPrice = stockRequest.CurrentPrice,
            Industry = stockRequest.Industry,
            MarketCap = stockRequest.MarketCap
        };
    }

    public Stock UpdateFromDto(Stock stock, StockUpdateRequestDto stockUpdateRequest) {
        if (stock == null || stockUpdateRequest == null) throw new ArgumentNullException(nameof(stock));

        stock.Symbol = string.IsNullOrWhiteSpace(stockUpdateRequest.Symbol)
            ? stock.Symbol
            : stockUpdateRequest.Symbol;

        stock.CompanyName = string.IsNullOrWhiteSpace(stockUpdateRequest.CompanyName)
            ? stock.CompanyName
            : stockUpdateRequest.CompanyName;

        if (stockUpdateRequest.CurrentPrice.HasValue)
            stock.CurrentPrice = stockUpdateRequest.CurrentPrice.Value;

        if (stockUpdateRequest.Industry.HasValue)
            stock.Industry = stockUpdateRequest.Industry.Value;

        if (stockUpdateRequest.MarketCap.HasValue)
            stock.MarketCap = stockUpdateRequest.MarketCap.Value;

        return stock;
    }
}