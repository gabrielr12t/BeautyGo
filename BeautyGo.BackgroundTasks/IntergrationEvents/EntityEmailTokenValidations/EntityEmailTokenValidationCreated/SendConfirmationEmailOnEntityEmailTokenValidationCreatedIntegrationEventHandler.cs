using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;
using BeautyGo.Domain.Repositories;
using BeautyGo.Domain.Settings;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.EntityEmailValidationTokens.EntityValidationTokenCreated;

internal class SendConfirmationEmailOnEntityEmailTokenValidationCreatedIntegrationEventHandler :
    IIntegrationEventHandler<EntityEmailValidationTokenCreatedIntegrationEvent>,
    IEntityValidationTokenHandle
{
    #region Fields

    private readonly IBaseRepository<UserEmailTokenValidation> _userEmailTokenValidationRepository;
    private readonly IBaseRepository<StoreEmailTokenValidation> _storeEmailTokenValidationRepository;
    private readonly IBaseRepository<BeautyGoEmailTokenValidation> _emailValidationTokenRepository;

    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApiSettings _apiSettings;

    #endregion

    #region Ctor

    public SendConfirmationEmailOnEntityEmailTokenValidationCreatedIntegrationEventHandler(
        IBaseRepository<UserEmailTokenValidation> userEmailTokenValidationRepository,
        IBaseRepository<StoreEmailTokenValidation> storeEmailTokenValidationRepository,
        IBaseRepository<BeautyGoEmailTokenValidation> emailValidationTokenRepository,
        IReceitaFederalIntegrationService receitaFederalIntegration,
        IEmailNotificationService emailNotificationService,
        IUnitOfWork unitOfWork,
        AppSettings appSettings)
    {
        _userEmailTokenValidationRepository = userEmailTokenValidationRepository;
        _storeEmailTokenValidationRepository = storeEmailTokenValidationRepository;
        _emailValidationTokenRepository = emailValidationTokenRepository;
        _emailNotificationService = emailNotificationService;
        _unitOfWork = unitOfWork;
        _apiSettings = appSettings.Get<ApiSettings>();
        _receitaFederalIntegration = receitaFederalIntegration;
    }

    #endregion

    public async Task Handle(EntityEmailValidationTokenCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var spec = new EntityByIdSpecification<BeautyGoEmailTokenValidation>(
            notification.BeautyGoEmailTokenId);

        var emailTokenValidation = await _emailValidationTokenRepository.GetFirstOrDefaultAsync(
            spec, cancellationToken: cancellationToken);

        if (emailTokenValidation == null)
            throw new DomainException(DomainErrors.EmailValidationToken.TokenNotFound);

        await emailTokenValidation.Handle(this);
    }

    #region Token validation implementations

    public async Task Handle(StoreEmailTokenValidation element)
    {
        var spec = new EntityByIdSpecification<StoreEmailTokenValidation>(element.Id)
            .AddInclude(p => p.Store);

        var storeEmailToken = await _storeEmailTokenValidationRepository.GetFirstOrDefaultAsync(spec);

        var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(storeEmailToken.Store.Cnpj);
        if (!cnpjReceitaFederalResponse.HasValue)
            throw new DomainException(DomainErrors.StoreEmailValidationToken.CnpjNotFound);

        var url = $"{_apiSettings.Host}/{_apiSettings.Endpoints.StoreConfirmEmail}?{storeEmailToken.Token}";

        var message = new StoreConfirmEmail(cnpjReceitaFederalResponse.Value.Email, storeEmailToken.Store.Name, url);

        await _emailNotificationService.SendAsync(message);
    }

    public async Task Handle(UserEmailTokenValidation element)
    {
        var spec = new EntityByIdSpecification<UserEmailTokenValidation>(element.Id)
            .AddInclude(p => p.User);

        var userEmailToken = await _userEmailTokenValidationRepository.GetFirstOrDefaultAsync(spec);

        var url = $"{_apiSettings.Host}/{_apiSettings.Endpoints.UserConfirmEmail}?{userEmailToken.Token}";

        var message = new UserConfirmEmail(userEmailToken.User.Email, userEmailToken.User.FullName(), url);

        await _emailNotificationService.SendAsync(message);
    }

    #endregion
}
