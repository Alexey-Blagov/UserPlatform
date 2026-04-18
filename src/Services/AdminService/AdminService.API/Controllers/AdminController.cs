using AdminService.Application.Queries.GetSystemSummary;
using AdminService.Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminPolicy")]
public sealed class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(SystemSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSystemSummaryQuery(), cancellationToken);
        return Ok(result.Value);
    }
}
