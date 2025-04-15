using portfolium.Core.Entities;
using portfolium.Web.Filters;

namespace portfolium.Core.Interfaces;

public interface IStockRepository {
    Task<List<Stock>> GetAllAsync(StockFilterRequest filter, CancellationToken ct);
    Task<Stock> GetByIdAsync(Guid id);
    Task<Stock> GetBySymbolAsync(string symbol);
    Task<Stock> AddAsync(Stock? stock, CancellationToken ct);
    Task<Stock> UpdateAsync(Stock stock, CancellationToken ct);
    Task DeleteAsync(Stock stock, CancellationToken ct);
}