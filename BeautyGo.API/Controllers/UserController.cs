using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Users.Commands.ConfirmEmailUser;
using BeautyGo.Application.Users.Commands.CreateUser;
using BeautyGo.Application.Users.Commands.UpdateUser;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Contracts.Users;
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
    [HttpPost("register/customer")]
    [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CreateUserRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
        .Bind(command => mediator.Send(new CreateUserCommand(request.FirstName, request.LastName, request.Email, request.Password, request.CPF, request.Phone, UserTypeEnum.Customer)))
        .Match(Ok, BadRequest);

    [AllowAnonymous]
    [HttpPost("register/professional")]
    [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterProfessional([FromBody] CreateUserRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
        .Bind(command => mediator.Send(new CreateUserCommand(request.FirstName, request.LastName, request.Email, request.Password, request.CPF, request.Password, UserTypeEnum.Professional)))
        .Match(Ok, BadRequest);

    [AllowAnonymous]
    [HttpPost("register/confirm")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token) =>
        await Result.Create(token, DomainErrors.General.UnProcessableRequest)
        .Bind(command => mediator.Send(new ConfirmEmailCommand(token)))
        .Match(Ok, BadRequest);

    [HttpPut("update")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand request) => 
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
        .Bind(command => mediator.Send(command))
        .Match(Ok, BadRequest); 
}
