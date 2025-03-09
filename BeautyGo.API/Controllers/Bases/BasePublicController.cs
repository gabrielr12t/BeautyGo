using MediatR;

namespace BeautyGo.Api.Controllers.Bases;

public class BasePublicController : BaseController
{
    public BasePublicController(IMediator mediator) : base(mediator)
    {
    }
}
