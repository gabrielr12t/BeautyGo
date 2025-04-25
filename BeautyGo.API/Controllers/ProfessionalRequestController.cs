
using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.ProfessionalRequests.AcceptProfessionalRequest;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.API.Controllers;

[Route("api/professional-requests")]
[Authorize]
public class ProfessionalRequestController : BasePublicController
{
    public ProfessionalRequestController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("invitation/{professionalRequestId:guid}/accept")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProfessionalInvitationRequestAccept(Guid professionalRequestId, CancellationToken cancellationToken) =>
        await Result.Create(new AcceptProfessionalRequestCommand(professionalRequestId), DomainErrors.General.UnProcessableRequest)
            .Bind(command => mediator.Send(command, cancellationToken))
            .Match(Ok, BadRequest);
}
