using Microsoft.EntityFrameworkCore;
using portfolium.Core.Entities;
using portfolium.Core.Extensions;
using portfolium.Core.Interfaces;
using portfolium.Infrastructure.Data;
using portfolium.Web.Filters;

namespace portfolium.Infrastructure.Repositories;

public class StockRepository(ApplicationDbContext applicationDbContext) : IStockRepository {
    public async Task<List<Stock?>> GetAllAsync(StockFilterRequest filter, CancellationToken ct) {
        var queryStock = applicationDbContext.Stock
                                             .AsNoTracking()
                                             .AsQueryable();

        queryStock = queryStock.ApplySort(filter).ApplyFilter(filter);

        var skipNumber = (filter.PageNumber - 1) * filter.PageSize;

        return await queryStock
                     .Skip(skipNumber)
                     .Take(filter.PageSize)
                     .ToListAsync(ct);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol) {
        return await applicationDbContext.Stock
                                         .FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock> AddAsync(Stock stock, CancellationToken ct) {
        await applicationDbContext.Stock.AddAsync(stock);
        await applicationDbContext.SaveChangesAsync(ct);
        return stock;
    }
}