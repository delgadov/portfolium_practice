using portfolium.Application.DTOs;
using portfolium.Application.Interfaces;
using portfolium.Core.Common;
using portfolium.Core.Errors;
using portfolium.Core.Interfaces;
using portfolium.Web.Filters;

namespace portfolium.Application.Services;

public class StockService(IStockRepository stockRepository, IStockMapper stockMapper) : IStockService {
    public async Task<Result<List<StockResponseDto>>> GetAllStocks(StockFilterRequest filter, CancellationToken ct) {
        var result = await stockRepository.GetAllAsync(filter, ct);
        var mapped = stockMapper.FromStocks(result);
        return Result<List<StockResponseDto>>.Success(mapped);
    }

    public async Task<Result<StockResponseDto>> AddStock(StockRequestDto stockRequest, CancellationToken ct) {
        var stock = stockMapper.FromRequestToStock(stockRequest);

        var stockExist = await stockRepository.GetBySymbolAsync(stockRequest.Symbol) != null;
        if (stockExist) return Result<StockResponseDto>.Fail(new StockAlreadyExistError(stockRequest.Symbol));

        await stockRepository.AddAsync(stock, ct);
        var mapped = stockMapper.FromStock(stock);
        return Result<StockResponseDto>.Success(mapped);
    }

    public async Task<Result<StockResponseDto>> UpdateStock(Guid id,
                                                            StockUpdateRequestDto stockUpdateRequest,
                                                            CancellationToken ct) {
        if (stockUpdateRequest is null) return Result<StockResponseDto>.Fail(new NullRequestError());

        var stock = await stockRepository.GetByIdAsync(id);
        if (stock == null) return Result<StockResponseDto>.Fail(new StockDoesNotExistError(id));

        stockMapper.UpdateFromDto(stock, stockUpdateRequest);

        if (stock.Symbol != stockUpdateRequest.Symbol) {
            var existingStock = await stockRepository.GetBySymbolAsync(stockUpdateRequest.Symbol);
            if (existingStock != null && existingStock.StockId != id)
                return Result<StockResponseDto>.Fail(new StockAlreadyExistError(stockUpdateRequest.Symbol));
        }

        await stockRepository.UpdateAsync(stock, ct);

        var mapped = stockMapper.FromStock(stock);
        return Result<StockResponseDto>.Success(mapped);
    }
}