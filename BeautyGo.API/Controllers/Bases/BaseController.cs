using BeautyGo.Domain.Core.Primitives;
using BeautyGo.Infrastructure.Contracts;
using BeautyGo.Infrastructure.Mvc.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers.Bases
{
    [HttpsRequirement]
    [Authorize]
    [ApiController]
    [SaveLastActivity]
    [SaveLastIpAddress]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;

        public BaseController(IMediator mediator) => this.mediator = mediator;

        protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse([error]));

        protected new IActionResult Ok(object value) => base.Ok(value);

        protected new IActionResult NotFound() => base.NotFound();
    }
}
