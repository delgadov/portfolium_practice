using portfolium.Application.DTOs;
using portfolium.Application.Interfaces;
using portfolium.Core.Entities;

namespace portfolium.Application.Mappers;

public class StockMapper : IStockMapper {
    public StockResponseDto FromStock(Stock stock) {
        if (stock == null) throw new ArgumentNullException(nameof(stock));

        return new StockResponseDto(
            stock.Symbol,
            stock.CompanyName,
            stock.CurrentPrice,
            stock.Industry,
            stock.MarketCap
        );
    }

    public List<StockResponseDto> FromStocks(List<Stock> stocks) {
        return stocks?.Select(FromStock).ToList() ?? [];
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
}