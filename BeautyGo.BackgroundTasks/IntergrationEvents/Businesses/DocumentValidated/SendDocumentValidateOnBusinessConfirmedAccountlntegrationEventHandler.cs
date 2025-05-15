using BeautyGo.Application.Businesses.Commands.DocumentValidated;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.DocumentValidated;

internal class SendDocumentValidateOnBusinessConfirmedAccountlntegrationEventHandler
    : IIntegrationEventHandler<BusinessDocumentValidatedIntegrationEvent>
{
    private readonly IBusinessEmailNotificationPublisher _businessEmailNotificationPublisher;
    private readonly IEFBaseRepository<Business> _businessRepository;

    public SendDocumentValidateOnBusinessConfirmedAccountlntegrationEventHandler(
        IEFBaseRepository<Business> businessRepository,
        IBusinessEmailNotificationPublisher businessEmailNotificationPublisher)
    {
        _businessRepository = businessRepository;
        _businessEmailNotificationPublisher = businessEmailNotificationPublisher;
    }

    public async Task Handle(BusinessDocumentValidatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var businessByIdSpecification = new EntityByIdSpecification<Business>(notification.BusinessId)
            .AddInclude(p => p.Owner);

        var business = await _businessRepository.GetFirstOrDefaultAsync(businessByIdSpecification);

        if (business == null)
            throw new DomainException(DomainErrors.Business.BusinessNotFound(notification.BusinessId));

        var message = new DocumentValidatedEmail(business.Owner.Email, business.Name, "LINK TESTE");

        await _businessEmailNotificationPublisher.PublishAsync(message, cancellationToken); 
    }
}
