using portfolium.Application.Enums;
using portfolium.Core.Entities;
using portfolium.Web.Filters;

namespace portfolium.Core.Extensions;

public static class StockQueryableExtension {
    public static IQueryable<Stock> ApplySort(this IQueryable<Stock> query, StockFilterRequest filter) {
        if (filter.SortStockBy.HasValue)
            query = filter.SortStockBy switch {
                SortStockBy.Symbol => filter.Descending
                    ? query.OrderByDescending(s => s.Symbol)
                    : query.OrderBy(s => s.Symbol),
                SortStockBy.CompanyName => filter.Descending
                    ? query.OrderByDescending(s => s.CompanyName)
                    : query.OrderBy(s => s.CompanyName),
                SortStockBy.CurrentPrice => filter.Descending
                    ? query.OrderByDescending(s => s.CurrentPrice)
                    : query.OrderBy(s => s.CurrentPrice),
                SortStockBy.Industry => filter.Descending
                    ? query.OrderByDescending(s => s.Industry)
                    : query.OrderBy(s => s.Industry),
                SortStockBy.MarketCap => filter.Descending
                    ? query.OrderByDescending(s => s.MarketCap)
                    : query.OrderBy(s => s.MarketCap),
                _ => query.OrderByDescending(s => s.CurrentPrice)
            };

        return query;
    }

    public static IQueryable<Stock> ApplyFilter(this IQueryable<Stock> query, StockFilterRequest filter) {
        if (!string.IsNullOrWhiteSpace(filter.Symbol))
            query = query.Where(s => s.Symbol == filter.Symbol);

        if (!string.IsNullOrWhiteSpace(filter.CompanyName))
            query = query.Where(s => s.CompanyName == filter.CompanyName);

        if (filter.MinCurrentPrice.HasValue)
            query = query.Where(s => s.CurrentPrice >= filter.MinCurrentPrice.Value);

        if (filter.MaxCurrentPrice.HasValue)
            query = query.Where(s => s.CurrentPrice <= filter.MaxCurrentPrice.Value);

        if (filter.Industry.HasValue)
            query = query.Where(s => s.Industry == filter.Industry);

        if (filter.MinMarketCap.HasValue)
            query = query.Where(s => s.MarketCap >= filter.MinMarketCap.Value);

        if (filter.MaxMarketCap.HasValue)
            query = query.Where(s => s.MarketCap <= filter.MaxMarketCap.Value);

        return query;
    }
}