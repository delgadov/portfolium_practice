using Microsoft.AspNetCore.JsonPatch;
using portfolium.Application.DTOs;
using portfolium.Application.Interfaces;
using portfolium.Application.Validators;
using portfolium.Core.Common;
using portfolium.Core.Configuration;
using portfolium.Core.Entities;
using portfolium.Core.Errors;
using portfolium.Core.Interfaces;
using portfolium.Web.Filters;

namespace portfolium.Application.Services;

public class StockService(IStockRepository stockRepository, IStockMapper stockMapper, IBulkSettings bulkSettings)
    : IStockService {
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

    public async Task<Result<BulkStockResponseDto>> AddStockBulk(List<StockRequestDto> stockRequestDtos,
                                                                 CancellationToken ct) {
        if (stockRequestDtos.Count == 0) return Result<BulkStockResponseDto>.Fail(new EmptyListError());
        Console.WriteLine(bulkSettings.MaxItemsPerRequest);
        if (stockRequestDtos.Count > bulkSettings.MaxItemsPerRequest)
            return Result<BulkStockResponseDto>.Fail(new RequestLimitError(bulkSettings.MaxItemsPerRequest));

        var notValidSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var validationErrorsList = new StockBulkRequestDtoValidator().Validate(stockRequestDtos);
        if (validationErrorsList.Count > 0)
            foreach (var error in validationErrorsList)
                notValidSet.Add(error.StockSymbol);

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var duplicatesHash = new HashSet<string>();
        var duplicatesRequestDtos = new List<StockRequestDto>();
        foreach (var dto in stockRequestDtos)
            if (!seen.Add(dto.Symbol)) {
                duplicatesHash.Add(dto.Symbol);
                duplicatesRequestDtos.Add(dto);
            }

        var dbSymbols = await stockRepository.GetAllSymbolsAsync();
        var dbSymbolSet = new HashSet<string>(dbSymbols, StringComparer.OrdinalIgnoreCase);
        if (dbSymbolSet.Count == 0)
            return Result<BulkStockResponseDto>.Fail(new DatabaseError("I could not find any Symbols in Database"));
        var existingInDb = stockRequestDtos
                           .Where(dto => dbSymbolSet.Contains(dto.Symbol))
                           .ToList();

        var existingDbSymbolSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var dto in existingInDb) existingDbSymbolSet.Add(dto.Symbol);

        stockRequestDtos.RemoveAll(dto =>
            duplicatesHash.Contains(dto.Symbol) || existingDbSymbolSet.Contains(dto.Symbol) ||
            notValidSet.Contains(dto.Symbol));

        var stockList = stockMapper.FromListRequestToStock(stockRequestDtos);
        await stockRepository.AddBulkAsync(stockList, ct);

        var mapped =
            stockMapper.FromBulkRequestToBulkResponse(duplicatesRequestDtos, existingInDb, stockList,
                validationErrorsList);

        return Result<BulkStockResponseDto>.Success(mapped,
            duplicatesHash.Any() || existingDbSymbolSet.Any() || notValidSet.Any()
                ? "Some errors ware identified while adding the objects"
                : "Success");
    }

    public async Task<Result<StockResponseDto>> UpdateStock(Guid id,
                                                            StockUpdateRequestDto stockUpdateRequest,
                                                            CancellationToken ct) {
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

    public async Task<Result<StockResponseDto>> PatchStock(
        Guid id, JsonPatchDocument<StockPatchRequestDto> jsonPatchDocument, CancellationToken ct) {
        var stock = await stockRepository.GetByIdAsync(id);
        if (stock == null) return Result<StockResponseDto>.Fail(new StockDoesNotExistError(id));

        var stockPatch = stockMapper.FromStockToPatch(stock);
        jsonPatchDocument.ApplyTo(stockPatch);

        var validationResult = await new StockPatchRequestDtoValidator().ValidateAsync(stockPatch, ct);
        if (!validationResult.IsValid)
            return Result<StockResponseDto>.Fail(
                new ValidationError(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));

        if (stock.Symbol != stockPatch.Symbol) {
            var existingStock = await stockRepository.GetBySymbolAsync(stockPatch.Symbol);
            if (existingStock != null && existingStock.StockId != id)
                return Result<StockResponseDto>.Fail(new StockAlreadyExistError(stockPatch.Symbol));
        }

        stockMapper.FromPatchToStock(stock, stockPatch);
        await stockRepository.UpdateAsync(stock, ct);

        var mapped = stockMapper.FromStock(stock);
        return Result<StockResponseDto>.Success(mapped);
    }

    public async Task<Result<bool>> DeleteStock(Guid id, CancellationToken ct) {
        var stock = await stockRepository.GetByIdAsync(id);
        if (stock == null) return Result<bool>.Fail(new StockDoesNotExistError(id));

        await stockRepository.DeleteAsync(stock, ct);

        return Result<bool>.Success(true);
    }

    public async Task<Result<BulkDeleteStockResponseDto>> DeleteStockBulk(BulkDeleteStockRequestDto bulkDeleteStockRequestDto, CancellationToken ct) {
        if (bulkDeleteStockRequestDto.StockIds == null || bulkDeleteStockRequestDto.StockIds.Count == 0)
            return Result<BulkDeleteStockResponseDto>.Fail(new EmptyListError());

        var stockList = await stockRepository.GetStocksByIdAsync(bulkDeleteStockRequestDto.StockIds, ct);
        if (stockList.Count == 0) return Result<BulkDeleteStockResponseDto>.Fail(new DatabaseError("I couldn't find any given stock"));

        var foundIds =  new HashSet<Guid>(stockList.Select(x => x.StockId));
        var notFoundIds = bulkDeleteStockRequestDto.StockIds.Except(foundIds).ToList();
        await stockRepository.DeleteBulkAsync(stockList, ct);
        var mapped = stockMapper.FromStockBulkDeleteToBulkResponse(foundIds.ToList(), notFoundIds);

        return Result<BulkDeleteStockResponseDto>.Success(mapped, foundIds.Count switch {
            > 0 when notFoundIds.Count > 0 => "Some objects were deleted some may not",
            0 when notFoundIds.Count > 0 => "Some errors were found while deleting the objects",
            _ => "Success"
        });
    }
}