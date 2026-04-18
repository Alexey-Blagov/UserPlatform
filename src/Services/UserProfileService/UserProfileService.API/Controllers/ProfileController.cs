using BuildingBlocks.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserProfileService.Application.Commands.UpsertMyAddress;
using UserProfileService.Application.Queries.GetMyProfile;
using UserProfileService.Contracts.Dtos;

namespace UserProfileService.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize(Policy = "UserPolicy")]
public sealed class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyProfileQuery(), cancellationToken);
        return ToActionResult(result);
    }

    [HttpPut("me/address")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpsertMyAddress([FromBody] UpsertAddressRequest request, CancellationToken cancellationToken)
    {
        var command = new UpsertMyAddressCommand(
            request.FirstName,
            request.LastName,
            request.Country,
            request.City,
            request.Street,
            request.House,
            request.PostalCode);

        var result = await _mediator.Send(command, cancellationToken);
        return ToActionResult(result);
    }

    private IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);

        return result.Error switch
        {
            "Unauthorized" => Unauthorized(),
            "Profile not found" => NotFound(),
            _ => BadRequest(new { error = result.Error })
        };
    }
}
