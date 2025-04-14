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
    [ResponseCache(Duration = 60)]
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
        return HandleResult(result, data => CreatedAtAction(nameof(GetAll), new { id = data.Id }, data));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Result<StockResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateStock(Guid id, [FromBody] StockUpdateRequestDto stockUpdateRequest,
                                                 CancellationToken ct) {
        var result = await stockService.UpdateStock(id, stockUpdateRequest, ct);
        return HandleResult(result, data => Ok(data));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteStock(Guid id, CancellationToken ct) {
        var result = await stockService.DeleteStock(id, ct);
        return HandleResult(result, _ => NoContent());
    }

    private IActionResult HandleResult<T>(Result<T> result, Func<T, IActionResult> onSuccess) {
        return result.IsSuccess
            ? onSuccess(result.Data)
            : StatusCode(result.ErrorResponse.StatusCode, result.ErrorResponse);
    }
}