using portfolium.Application.DTOs;
using portfolium.Core.Common;
using portfolium.Web.Filters;

namespace portfolium.Core.Interfaces;

public interface IStockService {
    Task<Result<List<StockResponseDto>>> GetAllStocks(StockFilterRequest filter, CancellationToken ct);
    Task<Result<StockResponseDto>> AddStock(StockRequestDto stockRequest, CancellationToken ct);
    Task<Result<StockResponseDto>> UpdateStock(Guid id, StockUpdateRequestDto stockUpdateRequest, CancellationToken ct);
    Task<Result<bool>> DeleteStock(Guid id, CancellationToken ct);
}