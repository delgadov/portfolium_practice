using Microsoft.EntityFrameworkCore;
using portfolium.Application.DTOs;
using portfolium.Core.Entities;
using portfolium.Core.Extensions;
using portfolium.Core.Interfaces;
using portfolium.Infrastructure.Data;
using portfolium.Web.Filters;

namespace portfolium.Infrastructure.Repositories;

public class StockRepository(ApplicationDbContext applicationDbContext) : IStockRepository {
    public async Task<List<Stock>> GetAllAsync(StockFilterRequest filter, CancellationToken ct) {
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

    public async Task<Stock> GetByIdAsync(Guid id) {
        return await applicationDbContext.Stock
                                         .FirstOrDefaultAsync(s => s.StockId == id);
    }

    public async Task<List<Stock>> GetStocksByIdAsync(List<Guid> stockIds, CancellationToken ct) {
        return await applicationDbContext.Stock
                                         .Where(s => stockIds.Contains(s.StockId))
                                         .ToListAsync();
    }

    public async Task<Stock> GetBySymbolAsync(string symbol) {
        return await applicationDbContext.Stock
                                         .FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<List<string>> GetAllSymbolsAsync() {
        return await applicationDbContext.Stock
                                         .Select(s => s.Symbol)
                                         .ToListAsync();
    }

    public async Task<Stock> AddAsync(Stock stock, CancellationToken ct) {
        await applicationDbContext.Stock.AddAsync(stock, ct);
        await applicationDbContext.SaveChangesAsync(ct);
        return stock;
    }

    public async Task<List<Stock>> AddBulkAsync(List<Stock> stockList, CancellationToken ct) {
        await applicationDbContext.Stock.AddRangeAsync(stockList, ct);
        await applicationDbContext.SaveChangesAsync(ct);
        return stockList;
    }

    public async Task<Stock> UpdateAsync(Stock stock, CancellationToken ct) {
        applicationDbContext.Stock.Update(stock);
        await applicationDbContext.SaveChangesAsync(ct);
        return stock;
    }

    public async Task DeleteAsync(Stock stock, CancellationToken ct) {
        applicationDbContext.Stock.Remove(stock);
        await applicationDbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteBulkAsync(List<Stock> stockList, CancellationToken ct) {
        applicationDbContext.Stock.RemoveRange(stockList);
        await applicationDbContext.SaveChangesAsync(ct);
    }
}