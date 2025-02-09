using MediatR;
using BeautyGo.Api.Controllers.Bases;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers;

[Route("api/[controller]")]
public class CatalogController : BasePublicController
{
    public CatalogController(IMediator mediator) : base(mediator)
    {
    }
}
