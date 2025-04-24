using portfolium.Application.DTOs;
using portfolium.Core.Entities;
using portfolium.Core.Errors;

namespace portfolium.Application.Interfaces;

public interface IStockMapper {
    StockResponseDto FromStock(Stock stock);
    List<StockResponseDto> FromStocks(List<Stock> stocks);
    Stock FromRequestToStock(StockRequestDto stockRequest);
    Stock UpdateFromDto(Stock stock, StockUpdateRequestDto stockUpdateRequest);
    StockPatchRequestDto FromStockToPatch(Stock stock);
    Stock FromPatchToStock(Stock stock, StockPatchRequestDto stockPatchRequest);
    List<Stock> FromListRequestToStock(List<StockRequestDto> stockRequestDtos);
    BulkStockResponseDto FromBulkRequestToBulkResponse(List<StockRequestDto> duplicatesRequests,
                                                       List<StockRequestDto> existingInDb,
                                                       List<Stock> successStock,
                                                       List<ValidationFailures> validationFailuresList);

    BulkDeleteStockResponseDto FromStockBulkDeleteToBulkResponse(List<Guid> foundIds, List<Guid> notFoundIds);
}