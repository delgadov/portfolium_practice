namespace portfolium.Application.DTOs;

public class BulkDeleteStockResponseDto {
    public List<Guid> DeletedStockIds { get; set; }
    public List<Guid> NotFound { get; set; }
}