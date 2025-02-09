using BeautyGo.Application.Business.Events.BusinessCreated;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Business;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;
using BeautyGo.Domain.Settings;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Business.BeautyBusinessCreated;

internal class SendConfirmationEmailOnBeautyBusinessCreatedIntegrationEventHandler : IIntegrationEventHandler<BeautyBusinessCreatedIntegrationEvent>
{
    private readonly IBaseRepository<BeautyBusiness> _businessRepository;
    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger _logger;

    public SendConfirmationEmailOnBeautyBusinessCreatedIntegrationEventHandler(IBaseRepository<BeautyBusiness> businessRepository,
        IReceitaFederalIntegrationService receitaFederalIntegration,
        AppSettings appSettings,
        ILogger logger)
    {
        _businessRepository = businessRepository;
        _receitaFederalIntegration = receitaFederalIntegration;
        _apiSettings = appSettings.Get<ApiSettings>();
        _logger = logger;
    }

    private string GenerateEmailConfirmLink(string token) =>
        $"{_apiSettings.Host}/{_apiSettings.Endpoints.BusinessConfirmEmail}?{token}";

    public async Task Handle(BeautyBusinessCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<BeautyBusiness>(notification.BusinessId));

        await _logger.InformationAsync($"Enviando confirmação de email do cnpj {business.Cnpj}");

        var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(business.Cnpj, cancellationToken);
        if (!cnpjReceitaFederalResponse.HasValue)
            throw new DomainException(DomainErrors.Business.InvalidCnpj);


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
