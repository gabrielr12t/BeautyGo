using BeautyGo.Api.Controllers.Bases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.API.Controllers;

[Route("api/[controller]")]
public class CustomerController : BaseController
{
    public CustomerController(IMediator mediator) : base(mediator)
    {
    }
}
