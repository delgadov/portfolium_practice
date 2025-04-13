using portfolium.Application.Enums;

namespace portfolium.Web.Filters;

public class StockFilterRequest {
    public string? Symbol { get; set; }
    public string? CompanyName { get; set; }
    public decimal? MinCurrentPrice { get; set; }
    public decimal? MaxCurrentPrice { get; set; }
    public IndustryType? Industry { get; set; }
    public long? MinMarketCap { get; set; }
    public long? MaxMarketCap { get; set; }
    public SortStockBy? SortStockBy { get; set; }
    public bool Descending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}