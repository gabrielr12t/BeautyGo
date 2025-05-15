using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.ProfessionalRequests.ProfessionalRequestAccepted;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestAccepted;

internal class NotifyBusinessOnProfessionalRequestAcceptedIntegrationEventHandler : IIntegrationEventHandler<ProfessionalRequestAcceptedIntegrationEvent>
{
    #region Fields

    private readonly IEFBaseRepository<ProfessionalRequest> _professionalRequestRepository;
    private readonly IBusinessEmailNotificationPublisher _notificationPublisher;

    #endregion

    #region Ctor

    public NotifyBusinessOnProfessionalRequestAcceptedIntegrationEventHandler(
        IEFBaseRepository<ProfessionalRequest> professionalRequestRepository,
        IBusinessEmailNotificationPublisher notificationPublisher)
    {
        _professionalRequestRepository = professionalRequestRepository;
        _notificationPublisher = notificationPublisher;
    }

    #endregion

    #region Utilities

    private async Task<ProfessionalRequest> GetProfessionalRequestByIdAsync(Guid professionalRequestId, CancellationToken cancellationToken)
    {
        var professionalRequestByIdSpec = new EntityByIdSpecification<ProfessionalRequest>(professionalRequestId)
            .AddInclude(p => p.Business)
            .AddInclude(p => p.User);

        return await _professionalRequestRepository.GetFirstOrDefaultAsync(professionalRequestByIdSpec, false, cancellationToken);
    }

    #endregion

    #region Handle

    public async Task Handle(ProfessionalRequestAcceptedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var professionalRequest = await GetProfessionalRequestByIdAsync(notification.ProfessionalRequestId, cancellationToken);

        if (professionalRequest == null)
            throw new DomainException(DomainErrors.ProfessionalRequest.NotFound);

        //VALIDAR OS OBJETOS (BUSINESS e USER) DENTRO DE 'professionalRequest'

        var message = new BusinessProfessionalAddedEmail(
            professionalRequest.Business.Email,
            professionalRequest.User.FullName(),
            professionalRequest.Business.Name);

        await _notificationPublisher.PublishAsync(message, cancellationToken);
    }

    #endregion
}
