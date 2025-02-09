using BeautyGo.Api.Controllers.Bases;
using BeautyGo.Application.Authentication.Commands.Login;
using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Security;
using BeautyGo.Application.Core.Abstractions.Stores;
using BeautyGo.Application.Users.Commands.CreateUser;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyGo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InitialTestController : BaseController
{
    private IStoreContext storeContext;
    private readonly IEncryptionService _encryptionService;
    private readonly IAuthService _authService;

    public InitialTestController(
        IMediator mediator,
        IStoreContext storeContext,
        IEncryptionService encryptionService,
        IAuthService authService) : base(mediator)
    {
        this.storeContext = storeContext;
        _encryptionService = encryptionService;
        _authService = authService;
    }

    public class MonitoramentoMemoria 
    {
        public int Uso { get; set; }
        public int Livre { get; set; }
    }

    public class DisplaySuporte
    {

    }

    [AllowAnonymous]
    [HttpGet("cadastrar")]
    public async Task<IActionResult> cadastrar()
    {
        var usuario = new CreateUserCommand("Gabriel", "Santos",
            "gabriel_ps15@hotmail.com", "rua12team", "43660600865");

        return Ok(await mediator.Send(usuario));
    }

    [HttpGet("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> login()
    {
        var request = new LoginCommand("gabriel_ps15@hotmail.com", "rua12team");

        return await Result.Create(request, DomainErrors.General.UnProcessableRequest)
           .Bind(command => mediator.Send(command))
           .Match(Ok, BadRequest);

        //var model = new LoginModel("gabriel@gmail.com", "12345");

        //var token = await _authService.AuthenticateAsync(model);

        //return Ok(token);
    }

    [HttpGet("authorize")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> TestarAuthorize()
    {
        var result = await _authService.GetCurrentUserAsync();

        if (result == null)
            return Ok(new { result = "Ninguém autenticado" });

        return Ok(new { result = "Usuário autenticado" });
    }
}