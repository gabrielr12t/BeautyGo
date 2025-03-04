using BeautyGo.Application.Businesses.Commands.DocumentValidated;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.DocumentValidated;

internal class SendDocumentValidateOnBusinessConfirmedAccountlntegrationEventHandler
    : IIntegrationEventHandler<BusinessDocumentValidatedIntegrationEvent>
{
    private readonly IBusinessEmailNotificationPublisher _businessEmailNotificationPublisher;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Business> _businessRepository;

    public SendDocumentValidateOnBusinessConfirmedAccountlntegrationEventHandler(
        ILogger logger,
        IUnitOfWork unitOfWork,
        IBaseRepository<Business> businessRepository,
        IBusinessEmailNotificationPublisher businessEmailNotificationPublisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _businessRepository = businessRepository;
        _businessEmailNotificationPublisher = businessEmailNotificationPublisher;
    }

    public async Task Handle(BusinessDocumentValidatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var businessByIdSpecification = new EntityByIdSpecification<Business>(notification.BusinessId);
        var business = await _businessRepository.GetFirstOrDefaultAsync(businessByIdSpecification);

        if (business == null)
            throw new DomainException(DomainErrors.Business.BusinessNotFound(notification.BusinessId));

        var message = new DocumentValidatedEmail(business.Email, business.Name, "LINK TESTE");

        await _businessEmailNotificationPublisher.PublishAsync(message, cancellationToken); ;
    }
}
