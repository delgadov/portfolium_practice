using portfolium.Application.DTOs;
using portfolium.Core.Entities;

namespace portfolium.Application.Interfaces;

public interface IStockMapper {
    StockResponseDto FromStock(Stock stock);
    List<StockResponseDto> FromStocks(List<Stock> stocks);
    Stock FromRequestToStock(StockRequestDto stockRequest);
}