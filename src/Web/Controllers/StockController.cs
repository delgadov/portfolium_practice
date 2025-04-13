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
        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<StockResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddStock([FromBody] StockRequestDto requestDto, CancellationToken ct) {
        var result = await stockService.AddStock(requestDto, ct);
        return HandleResult(result);
    }

    private IActionResult HandleResult<T>(Result<T> result) {
        return result.IsSuccess
            ? Ok(result.Data)
            : StatusCode(result.ErrorResponse.StatusCode, result.ErrorResponse);
    }
}