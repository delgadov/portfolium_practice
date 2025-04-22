using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using portfolium.Application.DTOs;
using portfolium.Core.Common;
using portfolium.Core.Interfaces;
using portfolium.Web.Filters;

namespace portfolium.Web.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController(IStockService stockService) : ControllerBase {
    [HttpGet]
    [ResponseCache(Duration = 1)]
    [ProducesResponseType(typeof(Result<List<StockResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] StockFilterRequest filter,
                                            CancellationToken ct) {
        var result = await stockService.GetAllStocks(filter, ct);
        return HandleResult(result, data => Ok(data));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<StockResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddStock([FromBody] StockRequestDto requestDto, CancellationToken ct) {
        var result = await stockService.AddStock(requestDto, ct);
        return HandleResult(result, result => CreatedAtAction(nameof(GetAll), new { id = result.Data.Id }, result));
    }

    [HttpPost("bulk")]
    [ProducesResponseType(typeof(Result<BulkStockResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<BulkStockResponseDto>), StatusCodes.Status206PartialContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddStockBulk([FromBody] BulkStockRequestDto bulkStockRequestDto,
                                                  CancellationToken ct) {
        var result = await stockService.AddStockBulk(bulkStockRequestDto.StockRequests, ct);
        return HandleResult(result, result => {
            var hasFailures = result.Data.ValidationFailures.Any()
                              || result.Data.DuplicatesStocks.Any()
                              || result.Data.ExistingDbStocks.Any();
            return StatusCode(hasFailures ? StatusCodes.Status206PartialContent : StatusCodes.Status201Created, result);
        });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Result<StockResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateStock(Guid id, [FromBody] StockUpdateRequestDto stockUpdateRequest,
                                                 CancellationToken ct) {
        if (id == Guid.Empty) return BadRequest(new { error = "Invalid stock ID" });
        var result = await stockService.UpdateStock(id, stockUpdateRequest, ct);
        return HandleResult(result, data => Ok(data));
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(Result<StockResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PatchStock(
        Guid id, [FromBody] JsonPatchDocument<StockPatchRequestDto> patchDocument, CancellationToken ct) {
        if (id == Guid.Empty) return BadRequest(new { error = "Invalid stock ID" });
        var result = await stockService.PatchStock(id, patchDocument, ct);
        return HandleResult(result, data => Ok(data));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteStock(Guid id, CancellationToken ct) {
        if (id == Guid.Empty) return BadRequest(new { error = "Invalid stock ID" });
        var result = await stockService.DeleteStock(id, ct);
        return HandleResult(result, _ => NoContent());
    }

    private IActionResult HandleResult<T>(Result<T> result, Func<Result<T>, IActionResult> onSuccess) {
        if (!result.IsSuccess) return StatusCode(result.ErrorResponse.StatusCode, result.ErrorResponse);

        var actionResult = onSuccess(result);
        result.StatusCode = actionResult switch {
            ObjectResult objectResult => objectResult.StatusCode ?? 200,
            StatusCodeResult statusCodeResult => statusCodeResult.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };

        return onSuccess(result);
    }
}