using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Businesses.Commands.CreateBusiness;
using BeautyGo.Application.Businesses.Commands.CreateWorkingHours;
using BeautyGo.Application.Businesses.Commands.SendProfessionalRequest;
using BeautyGo.Application.EmailValidationToken.EmailTokenValidationValidate;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using BeautyGo.Infrastructure.Mvc.Filter;
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
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CreateBusinessCommand command) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command))
            .Match(Ok, BadRequest);

    [HttpPost("working-hours")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterWorkingHours([FromBody] CreateWorkingHoursCommand command, CancellationToken cancellationToken) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command, cancellationToken))
            .Match(Ok, BadRequest);

    [AllowAnonymous]
    [HttpGet("register/confirm")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmAccount([FromQuery] string token) =>
       await Result.Create(token, DomainErrors.General.UnProcessableRequest)
       .Bind(command => mediator.Send(new ConfirmAccountCommand(token)))
       .Match(Ok, BadRequest);

    [AuthorizeOwner]
    [HttpPost("professional/invitation")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProfessionalInvitationRequest([FromBody] SendProfessionalRequestCommand command, CancellationToken cancellationToken) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command, cancellationToken))
            .Match(Ok, BadRequest);

    //CONTINUAR ............................................................

    //[HttpPost("professional/invitation/{userId:guid}/accept")]
    //[ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> ProfessionalInvitationRequestAccept(CancellationToken cancellationToken) =>
    //    await Result.Create(new { }, DomainErrors.General.UnProcessableRequest)
    //        .Bind(command => mediator.Send(command, cancellationToken))
    //        .Match(Ok, BadRequest);
}
