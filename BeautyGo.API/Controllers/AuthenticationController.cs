using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Authentication.Commands.Login;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers;

[Route("api/account")]
public class AuthenticationController : BaseController
{
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    } 

    [AllowAnonymous]
    [HttpPost("auth")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Auth([FromBody] LoginCommand request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command))
            .Match(Ok, BadRequest); 
}
