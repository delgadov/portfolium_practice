using portfolium.Application.DTOs;
using portfolium.Application.Interfaces;
using portfolium.Core.Entities;
using portfolium.Core.Errors;

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

        stock.Symbol = stockUpdateRequest.Symbol;
        stock.CompanyName = stockUpdateRequest.CompanyName;
        stock.CurrentPrice = stockUpdateRequest.CurrentPrice;
        stock.Industry = stockUpdateRequest.Industry;
        stock.MarketCap = stockUpdateRequest.MarketCap;

        return stock;
    }

    public StockPatchRequestDto FromStockToPatch(Stock stock) {
        return new StockPatchRequestDto {
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            CurrentPrice = stock.CurrentPrice,
            Industry = stock.Industry,
            MarketCap = stock.MarketCap
        };
    }

    public Stock FromPatchToStock(Stock stock, StockPatchRequestDto patchRequest) {
        stock.Symbol = patchRequest.Symbol;
        stock.CompanyName = patchRequest.CompanyName;
        stock.CurrentPrice = patchRequest.CurrentPrice;
        stock.Industry = patchRequest.Industry;
        stock.MarketCap = patchRequest.MarketCap;
        return stock;
    }

    public List<Stock> FromListRequestToStock(List<StockRequestDto> stockRequestDtos) {
        if (stockRequestDtos == null) throw new ArgumentNullException(nameof(stockRequestDtos));

        return stockRequestDtos.Select(FromRequestToStock)
                               .ToList();
    }

    public BulkStockResponseDto FromBulkRequestToBulkResponse(List<StockRequestDto> duplicatesRequests,
                                                              List<StockRequestDto> existingInDb,
                                                              List<Stock> successStock,
                                                              List<ValidationFailures> validationFailuresList) {
        return new BulkStockResponseDto {
            SuccessStocks = FromStocks(successStock),
            DuplicatesStocks = duplicatesRequests,
            ExistingDbStocks = existingInDb,
            ValidationFailures = validationFailuresList
        };
    }

    public BulkDeleteStockResponseDto FromStockBulkDeleteToBulkResponse(List<Guid> foundIds, List<Guid> notFoundIds) {
        return new BulkDeleteStockResponseDto {
            DeletedStockIds = foundIds,
            NotFound = notFoundIds,
        };
    }
}