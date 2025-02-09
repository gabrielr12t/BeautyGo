using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using Polly;

namespace BeautyGo.Infrastructure.Mvc.HttpServices.DelegatingHandlers;

public class RetryDelegatingHandler : DelegatingHandler
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public RetryDelegatingHandler(
        ILogger logger,
        IUnitOfWork unitOfWork,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _authService = authService;
    }

    #region Utilities

    private IAsyncPolicy<HttpResponseMessage> GetAsyncPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetryAsync: OnRetry);
    }

    private async Task OnRetry(DelegateResult<HttpResponseMessage> outcome, TimeSpan timespan, int retryCount, Context context)
    {
        var requestId = outcome.Result.RequestMessage.Headers.GetValues("X-Request-ID").FirstOrDefault();

        if (outcome.Exception != null)
        {
            await _logger.WarningAsync(
                $"{requestId} | Tentativa {retryCount}: Exceção ao enviar requisição. " +
                $" | URL: {context["RequestUri"]}. " +
                $" | Aguardando {timespan.TotalSeconds} segundos antes da próxima tentativa.",
                user: await _authService.GetCurrentUserAsync());
        }
        else
        {
            await _logger.WarningAsync(
                $"{requestId} | Tentativa {retryCount}. Resposta com falha ao enviar requisição. " +
                $" | URL: {outcome.Result.RequestMessage.RequestUri.AbsoluteUri}, StatusCode: {outcome.Result.StatusCode}, " +
                $" | Aguardando {timespan.TotalSeconds} segundos antes da próxima tentativa.",
                user: await _authService.GetCurrentUserAsync());
        }
    }

    private async Task<HttpResponseMessage> HandleResponseMessageAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestId = request.Headers.GetValues("X-Request-ID").FirstOrDefault();

        var result = await base.SendAsync(request, cancellationToken);

        if (!result.IsSuccessStatusCode)
        {
            var content = await result.Content?.ReadAsStringAsync(cancellationToken);

            await _logger.WarningAsync($"{requestId} | Resposta com falha. URL: {request.RequestUri}, " +
                               $"StatusCode: {result.StatusCode}, Conteúdo: {content}", user: await _authService.GetCurrentUserAsync());
        }

        return result;
    }

    #endregion

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetAsyncPolicy().ExecuteAsync(async () =>
                await HandleResponseMessageAsync(request, cancellationToken).ConfigureAwait(false));

            return response;
        }
        finally
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}