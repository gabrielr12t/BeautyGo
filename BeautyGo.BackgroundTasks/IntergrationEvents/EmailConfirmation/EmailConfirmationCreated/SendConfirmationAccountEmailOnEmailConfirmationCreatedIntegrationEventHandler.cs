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
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;
using BeautyGo.Domain.Repositories.Bases;
using BeautyGo.Domain.Settings;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.EntityEmailTokenValidations.EntityEmailTokenValidationCreated;

public class SendConfirmationAccountEmailOnEmailConfirmationCreatedIntegrationEventHandler :
    IIntegrationEventHandler<EmailConfirmationCreatedIntegrationEvent>,
    IEntityConfirmationHandle
{
    #region Fields

    private readonly IEFBaseRepository<UserEmailConfirmation> _userEmailTokenValidationRepository;
    private readonly IEFBaseRepository<BusinessEmailConfirmation> _businessEmailTokenValidationRepository;
    private readonly IEFBaseRepository<EmailConfirmation> _emailValidationTokenRepository;
    private readonly IEFBaseRepository<EmailNotification> _emailRespotory;

    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly IUserEmailNotificationPublisher _userEmailNotificationPublisher;
    private readonly IBusinessEmailNotificationPublisher _businessEmailNotificationPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApiSettings _apiSettings;

    #endregion

    #region Ctor

    public SendConfirmationAccountEmailOnEmailConfirmationCreatedIntegrationEventHandler(
        IEFBaseRepository<UserEmailConfirmation> userEmailTokenValidationRepository,
        IEFBaseRepository<BusinessEmailConfirmation> businessEmailTokenValidationRepository,
        IEFBaseRepository<EmailConfirmation> emailValidationTokenRepository,
        IReceitaFederalIntegrationService receitaFederalIntegration,
        IUnitOfWork unitOfWork,
        AppSettings appSettings,
        IUserEmailNotificationPublisher userEmailNotificationPublisher,
        IBusinessEmailNotificationPublisher businessEmailNotificationPublisher,
        IEFBaseRepository<EmailNotification> emailRespotory)
    {
        _userEmailTokenValidationRepository = userEmailTokenValidationRepository;
        _businessEmailTokenValidationRepository = businessEmailTokenValidationRepository;
        _emailValidationTokenRepository = emailValidationTokenRepository;
        _unitOfWork = unitOfWork;
        _apiSettings = appSettings.Get<ApiSettings>();
        _receitaFederalIntegration = receitaFederalIntegration;
        _userEmailNotificationPublisher = userEmailNotificationPublisher;
        _businessEmailNotificationPublisher = businessEmailNotificationPublisher;
        _emailRespotory = emailRespotory;
    }

    #endregion

    public async Task Handle(EmailConfirmationCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var spec = new EntityByIdSpecification<EmailConfirmation>(
            notification.BeautyGoEmailTokenId);

        var emailTokenValidation = await _emailValidationTokenRepository.GetFirstOrDefaultAsync(
            spec, cancellationToken: cancellationToken);

        if (emailTokenValidation == null)
            throw new DomainException(DomainErrors.EmailValidationToken.TokenNotFound);

        await emailTokenValidation.Handle(this);
    }

    #region Token validation implementations

    public async Task Handle(BusinessEmailConfirmation element)
    {
        var spec = new EntityByIdSpecification<BusinessEmailConfirmation>(element.Id)
            .AddInclude(p => p.Business);

        var businessEmailToken = await _businessEmailTokenValidationRepository.GetFirstOrDefaultAsync(spec);

        var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(businessEmailToken.Business.Cnpj);
        if (!cnpjReceitaFederalResponse.HasValue)
            throw new DomainException(DomainErrors.BusinessEmailValidationToken.CnpjNotFound);

        var url = $"{_apiSettings.Host}/{_apiSettings.Endpoints.BusinessConfirmEmail}?token={businessEmailToken.Token}";

        var message = new ConfirmAccountEmail(cnpjReceitaFederalResponse.Value.Email, businessEmailToken.Business.Name, url);

        await _businessEmailNotificationPublisher.PublishAsync(message);
    }

    public async Task Handle(UserEmailConfirmation element)
    {
        var spec = new EntityByIdSpecification<UserEmailConfirmation>(element.Id)
            .AddInclude(p => p.User);

        var userEmailToken = await _userEmailTokenValidationRepository.GetFirstOrDefaultAsync(spec);

        var url = $"{_apiSettings.Host}/{_apiSettings.Endpoints.UserConfirmEmail}?token={userEmailToken.Token}";

        var message = new ConfirmAccountEmail(userEmailToken.User.Email, userEmailToken.User.FullName(), url);

        await _userEmailNotificationPublisher.PublishAsync(message);
    }

    #endregion
}
