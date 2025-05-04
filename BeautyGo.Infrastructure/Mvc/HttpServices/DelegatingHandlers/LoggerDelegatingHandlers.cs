using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;

namespace BeautyGo.Infrastructure.Mvc.HttpServices.DelegatingHandlers;

public class LoggerDelegatingHandler : DelegatingHandler
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public LoggerDelegatingHandler(
        ILogger logger,
        IUnitOfWork unitOfWork,
        IAuthService authService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestId = Guid.NewGuid().ToString();

        try
        {
            request.Headers.Add("X-Request-ID", requestId);

            var requestContent = await (request.Content?.ReadAsStringAsync(cancellationToken) ?? Task.FromResult(string.Empty));

            await _logger.InformationAsync($"{requestId} | Enviando requisição para {request.RequestUri} | {requestContent}",
                user: await _authService.GetCurrentUserAsync(cancellationToken));

            var response = await base.SendAsync(request, cancellationToken);

            var responseContent = await response.Content?.ReadAsStringAsync(cancellationToken);

            await _logger.InformationAsync($"{requestId} | Resposta da requisição {request.RequestUri} | StatusCode:{response.StatusCode} | {responseContent}",
                user: await _authService.GetCurrentUserAsync(cancellationToken));

            return response;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"{requestId} | Erro crítico ao enviar requisição para {request.RequestUri}", ex, user: await _authService.GetCurrentUserAsync());
            throw;
        }
        finally
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
