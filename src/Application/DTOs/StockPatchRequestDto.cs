using portfolium.Application.Enums;

namespace portfolium.Application.DTOs;

public class StockPatchRequestDto {
    public string Symbol { get; set; }
    public string CompanyName { get; set; }
    public decimal CurrentPrice { get; set; }
    public IndustryType Industry { get; set; }
    public long MarketCap { get; set; }
}