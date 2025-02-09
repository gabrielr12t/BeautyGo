using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Business.Commands.CreateBusiness;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers;

[Route("api/[controller]")]
public class StoreController : BasePublicController
{
    public StoreController(IMediator mediator) : base(mediator)
    {
    }

    [Authorize]
    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CreateBeautyBusinessCommand command) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command))
            .Match(Ok, BadRequest);

    [HttpGet("{storeHost}")]
    public async Task<IActionResult> Details(string storeHost)
    {
        await Task.CompletedTask;

        return Ok(new { host = storeHost });
    }
}
