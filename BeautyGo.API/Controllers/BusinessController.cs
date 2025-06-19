using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Businesses.Commands.CreateBusiness;
using BeautyGo.Application.Businesses.Commands.CreateWorkingHours;
using BeautyGo.Application.Businesses.Commands.SendProfessionalRequest;
using BeautyGo.Application.Businesses.Queries.GetBusinessesFilter;
using BeautyGo.Application.EmailValidationToken.EmailConfirmationValidate;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using BeautyGo.Infrastructure.Mvc.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers;

[Route("api/[controller]")] 
public class BusinessController : BaseController
{
    public BusinessController(IMediator mediator) : base(mediator)
    {
    }

    #region Command

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CreateBusinessCommand command, CancellationToken cancellationToken) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command, cancellationToken))
            .Match(Ok, BadRequest);

    [HttpGet("register/confirm")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmAccount([FromQuery] string token, CancellationToken cancellationToken) =>
       await Result.Create(token, DomainErrors.General.UnProcessableRequest)
       .Bind(command => mediator.Send(new ConfirmAccountCommand(token), cancellationToken))
       .Match(Ok, BadRequest);

    [HttpPost("working-hours")]
    [AuthorizeOwner]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterWorkingHours([FromBody] CreateWorkingHoursCommand command, CancellationToken cancellationToken) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command, cancellationToken))
            .Match(Ok, BadRequest);
    
    [HttpPost("professional/invitation")]
    [AuthorizeOwner]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProfessionalInvitationRequest([FromBody] SendProfessionalRequestCommand command, CancellationToken cancellationToken) =>
        await Result.Create(command, DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command, cancellationToken))
            .Match(Ok, BadRequest);

    #endregion

    #region Query

    [HttpGet("filter")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> FilterBusiness([FromQuery] GetBusinessesFilterPagedQuery query) =>
        Ok(await mediator.Send(query));

    #endregion
}

