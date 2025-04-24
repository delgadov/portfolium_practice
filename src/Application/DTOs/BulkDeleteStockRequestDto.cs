namespace portfolium.Application.DTOs;

public class BulkDeleteStockRequestDto {
    public List<Guid> StockIds { get; set; }
}