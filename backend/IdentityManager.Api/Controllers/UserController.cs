using IdentityManager.Application.Dtos;
using IdentityManager.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<object>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 409)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage);

            return BadRequest(new { errors = errors.ToList() });
        }

        await _mediator.Send(model);

        return CreatedAtAction(nameof(Register), new ApiResponse<object>
        {
            Message = "User registered successfully",
        });
    }
}
