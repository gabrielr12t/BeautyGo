using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Businesses.Commands.CreateBusiness;
using BeautyGo.Application.EmailValidationToken.EmailTokenValidationValidate;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class BusinessController : BasePublicController
{
    public BusinessController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CreateBusinessCommand command) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command))
            .Match(Ok, BadRequest);

    [AllowAnonymous]
    [HttpGet("register/confirm")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmAccount([FromQuery] string token) =>
       await Result.Create(token, DomainErrors.General.UnProcessableRequest)
       .Bind(command => mediator.Send(new ConfirmAccountCommand(token)))
       .Match(Ok, BadRequest);

    [HttpGet("{storeHost}")]
    public async Task<IActionResult> Details(string storeHost)
    {
        await Task.CompletedTask;

        return Ok(new { host = storeHost });
    }
}
