using BeautyGo.Domain.Core.Primitives;
using BeautyGo.Infrastructure.Contracts;
using BeautyGo.Infrastructure.Mvc.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BeautyGo.Api.Controllers.Bases
{
    [EnableRateLimiting("fixed")]
    [Authorize]
    [ApiController]
    [SaveLastActivity]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;

        public BaseController(IMediator mediator) => this.mediator = mediator;

        protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));

        protected new IActionResult Ok(object value) => base.Ok(value);

        protected new IActionResult NotFound() => base.NotFound();
    }
}
