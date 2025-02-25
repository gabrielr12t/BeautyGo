using BeautyGo.Application.Businesses.Events.BusinessCreated;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;
using BeautyGo.Domain.Settings;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.BusinessCreated;

//DEVE FAZER HANDLE APENAS QUANDO O DOCUMENTO FOR VALIDADO, BusinessDocumentValidatedEvent
internal class SendFirstWelcomeEmailOnBusinessCreatedIntegrationEventHandler : IIntegrationEventHandler<BusinessCreatedIntegrationEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Business> _businessRepository;
    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger _logger;

    public SendFirstWelcomeEmailOnBusinessCreatedIntegrationEventHandler(
        IBaseRepository<Business> businessRepository,
        IReceitaFederalIntegrationService receitaFederalIntegration,
        AppSettings appSettings,
        ILogger logger,
        IUnitOfWork unitOfWork)
    {
        _businessRepository = businessRepository;
        _receitaFederalIntegration = receitaFederalIntegration;
        _apiSettings = appSettings.Get<ApiSettings>();
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    //private string GenerateEmailConfirmLink(string token) =>
    //    $"{_apiSettings.Host}/{_apiSettings.Endpoints.BusinessConfirmEmail}?{token}";

    public async Task Handle(BusinessCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetFirstOrDefaultAsync(new EntityByIdSpecification<Business>(notification.BusinessId));

        await _logger.InformationAsync($"Enviando de primeiras boas vindas ao cnpj criado: {business.Cnpj}");

        //EMAIL APENAS DE BOAS VINDAS, DIZENDO QUE A LOJA FOI CRIADA E PASSARÁ POR VALIDAÇÃO INTERNA.

        await _unitOfWork.SaveChangesAsync();

















        //var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(business.Cnpj, cancellationToken);
        //if (!cnpjReceitaFederalResponse.HasValue)
        //    throw new DomainException(DomainErrors.Business.InvalidCnpj(business.Cnpj));




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
