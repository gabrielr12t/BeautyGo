using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Users.Commands.ConfirmEmailUser;
using BeautyGo.Application.Users.Commands.CreateUser;
using BeautyGo.Application.Users.Commands.UpdateUser;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers;

[Route("api/[controller]")]
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CreateUserCommand command) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command))
            .Match(Ok, BadRequest);

    [HttpPut("update")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand request) =>
       await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command))
            .Match(Ok, BadRequest);

    [AllowAnonymous]
    [HttpPost("confirm-email")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token) =>
        await Result.Create(token, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(new ConfirmEmailCommand(token)))
            .Match(Ok, BadRequest);

}
