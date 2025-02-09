using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Stores.Events.StoreCreated;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;
using BeautyGo.Domain.Settings;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Stores.StoreCreated;

internal class SendConfirmationEmailOnStoreCreatedIntegrationEventHandler : IIntegrationEventHandler<StoreCreatedIntegrationEvent>
{
    private readonly IBaseRepository<Store> _storeRepository;
    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger _logger;

    public SendConfirmationEmailOnStoreCreatedIntegrationEventHandler(IBaseRepository<Store> storeRepository,
        IReceitaFederalIntegrationService receitaFederalIntegration,
        AppSettings appSettings,
        ILogger logger)
    {
        _storeRepository = storeRepository;
        _receitaFederalIntegration = receitaFederalIntegration;
        _apiSettings = appSettings.Get<ApiSettings>();
        _logger = logger;
    }

    private string GenerateEmailConfirmLink(string token) =>
        $"{_apiSettings.Host}/{_apiSettings.Endpoints.StoreConfirmEmail}?{token}";

    public async Task Handle(StoreCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Store>(notification.StoreId));

        await _logger.InformationAsync($"Enviando confirmação de email do cnpj {store.Cnpj}");

        var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(store.Cnpj, cancellationToken);
        if (!cnpjReceitaFederalResponse.HasValue)
            throw new DomainException(DomainErrors.Store.InvalidCnpj);


        //CONTINUAR ...

        //ENVIAR EMAIL PARA O EMAIL QUE ESTA NA RECEITA PARA SER VALIDADO SE A LOJA PODE SER ABERTA, 
        //REMOVENDO A POSSIBILIDADE DE FRAUDE

        //var spec = new EntityByIdSpecification<UserEmailValidationToken>(
        //    notification.UserEmailValidationTokenId).AddInclude(p => p.User);

        //var userEmailToken = await _userEmailValidationTokenRepository.GetFirstOrDefaultAsync(spec);

        //if (userEmailToken == null)
        //    throw new DomainException(DomainErrors.UserEmailValidationToken.TokenNotFound);

        //var message = new ConfirmEmail(userEmailToken.User.Email, userEmailToken.User.FullName(), GenerateEmailConfirmLink(userEmailToken.Token));

        //await _emailNotificationService.SendAsync(message);
    }
}
