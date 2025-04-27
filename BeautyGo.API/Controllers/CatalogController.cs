using MediatR;
using BeautyGo.Api.Controllers.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BeautyGo.Api.Controllers;

[Route("api/[controller]")]
public class CatalogController : BasePublicController
{
    public CatalogController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("ping")]
    [AllowAnonymous]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}
