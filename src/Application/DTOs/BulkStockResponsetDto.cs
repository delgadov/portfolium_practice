using portfolium.Core.Errors;

namespace portfolium.Application.DTOs;

public class BulkStockResponseDto {
    public List<StockResponseDto> SuccessStocks { get; set; } = [];
    public List<StockRequestDto> DuplicatesStocks { get; set; } = [];
    public List<StockRequestDto> ExistingDbStocks { get; set; } = [];
    public List<ValidationFailures> ValidationFailures { get; set; } = [];
}