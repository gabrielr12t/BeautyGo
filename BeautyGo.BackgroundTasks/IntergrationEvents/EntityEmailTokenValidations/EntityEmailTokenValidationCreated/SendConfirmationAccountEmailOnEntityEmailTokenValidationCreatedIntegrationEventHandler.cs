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

public class SendConfirmationAccountEmailOnEntityEmailTokenValidationCreatedIntegrationEventHandler :
    IIntegrationEventHandler<EmailValidationTokenCreatedIntegrationEvent>,
    IEntityValidationTokenHandle
{
    #region Fields

    private readonly IEFBaseRepository<UserEmailTokenValidation> _userEmailTokenValidationRepository;
    private readonly IEFBaseRepository<BusinessEmailTokenValidation> _businessEmailTokenValidationRepository;
    private readonly IEFBaseRepository<EmailTokenValidation> _emailValidationTokenRepository;
    private readonly IEFBaseRepository<EmailNotification> _emailRespotory;

    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly IUserEmailNotificationPublisher _userEmailNotificationPublisher;
    private readonly IBusinessEmailNotificationPublisher _businessEmailNotificationPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApiSettings _apiSettings;

    #endregion

    #region Ctor

    public SendConfirmationAccountEmailOnEntityEmailTokenValidationCreatedIntegrationEventHandler(
        IEFBaseRepository<UserEmailTokenValidation> userEmailTokenValidationRepository,
        IEFBaseRepository<BusinessEmailTokenValidation> businessEmailTokenValidationRepository,
        IEFBaseRepository<EmailTokenValidation> emailValidationTokenRepository,
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

    public async Task Handle(EmailValidationTokenCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var spec = new EntityByIdSpecification<EmailTokenValidation>(
            notification.BeautyGoEmailTokenId);

        var emailTokenValidation = await _emailValidationTokenRepository.GetFirstOrDefaultAsync(
            spec, cancellationToken: cancellationToken);

        if (emailTokenValidation == null)
            throw new DomainException(DomainErrors.EmailValidationToken.TokenNotFound);

        await emailTokenValidation.Handle(this);
    }

    #region Token validation implementations

    public async Task Handle(BusinessEmailTokenValidation element)
    {
        var spec = new EntityByIdSpecification<BusinessEmailTokenValidation>(element.Id)
            .AddInclude(p => p.Business);

        var businessEmailToken = await _businessEmailTokenValidationRepository.GetFirstOrDefaultAsync(spec);

        var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(businessEmailToken.Business.Cnpj);
        if (!cnpjReceitaFederalResponse.HasValue)
            throw new DomainException(DomainErrors.BusinessEmailValidationToken.CnpjNotFound);

        var url = $"{_apiSettings.Host}/{_apiSettings.Endpoints.BusinessConfirmEmail}?token={businessEmailToken.Token}";

        var message = new ConfirmAccountEmail(cnpjReceitaFederalResponse.Value.Email, businessEmailToken.Business.Name, url);

        await _businessEmailNotificationPublisher.PublishAsync(message);
    }

    public async Task Handle(UserEmailTokenValidation element)
    {
        var spec = new EntityByIdSpecification<UserEmailTokenValidation>(element.Id)
            .AddInclude(p => p.User);

        var userEmailToken = await _userEmailTokenValidationRepository.GetFirstOrDefaultAsync(spec);

        var url = $"{_apiSettings.Host}/{_apiSettings.Endpoints.UserConfirmEmail}?token={userEmailToken.Token}";

        var message = new ConfirmAccountEmail(userEmailToken.User.Email, userEmailToken.User.FullName(), url);

        await _userEmailNotificationPublisher.PublishAsync(message);
    }

    #endregion
}
