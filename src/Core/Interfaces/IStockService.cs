using portfolium.Application.DTOs;
using portfolium.Core.Common;
using portfolium.Web.Filters;

namespace portfolium.Core.Interfaces;

public interface IStockService {
    Task<Result<List<StockResponseDto>>> GetAllStocks(StockFilterRequest filter, CancellationToken ct);
    Task<Result<StockResponseDto>> AddStock(StockRequestDto stockRequest, CancellationToken ct);
}