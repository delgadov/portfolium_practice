using Microsoft.AspNetCore.JsonPatch;
using portfolium.Application.DTOs;
using portfolium.Core.Common;
using portfolium.Web.Filters;

namespace portfolium.Core.Interfaces;

public interface IStockService {
    Task<Result<List<StockResponseDto>>> GetAllStocks(StockFilterRequest filter, CancellationToken ct);
    Task<Result<StockResponseDto>> AddStock(StockRequestDto stockRequest, CancellationToken ct);
    Task<Result<BulkStockResponseDto>> AddStockBulk(List<StockRequestDto> stockRequestDtos, CancellationToken ct);
    Task<Result<StockResponseDto>> UpdateStock(Guid id, StockUpdateRequestDto stockUpdateRequest, CancellationToken ct);
    Task<Result<StockResponseDto>> PatchStock(Guid id, JsonPatchDocument<StockPatchRequestDto> jsonPatchDocument, CancellationToken ct);
    Task<Result<bool>> DeleteStock(Guid id, CancellationToken ct);
    Task<Result<BulkDeleteStockResponseDto>> DeleteStockBulk(BulkDeleteStockRequestDto bulkDeleteStockRequestDto, CancellationToken ct);
}