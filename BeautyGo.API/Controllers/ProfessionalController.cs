using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Infrastructure.Mvc.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.API.Controllers;

[Route("api/[controller]")]
[BeautyGoAuthorize(BeautyGoUserRoleDefaults.PROFESSIONAL)]
public class ProfessionalController : BaseController
{
    public ProfessionalController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        await Task.CompletedTask;

        return Ok();
    }
}
