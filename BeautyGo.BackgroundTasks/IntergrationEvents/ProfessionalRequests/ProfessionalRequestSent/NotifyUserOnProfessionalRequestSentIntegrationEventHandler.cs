using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestSent;

internal class NotifyUserOnProfessionalRequestSentIntegrationEventHandler
    : IIntegrationEventHandler<ProfessionalRequestSentIntegrationEvent>
{
    #region Fields

    private readonly IBaseRepository<ProfessionalRequest> _professionalRequestRepository;
    private readonly IProfessionalEmailNotificationPublisher _notificationPublisher;

    #endregion

    #region Ctor

    public NotifyUserOnProfessionalRequestSentIntegrationEventHandler(
        IBaseRepository<ProfessionalRequest> professionalRequestRepository,
        IProfessionalEmailNotificationPublisher notificationPublisher)
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

    public async Task Handle(ProfessionalRequestSentIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var professionalRequest = await GetProfessionalRequestByIdAsync(notification.ProfessionalRequestId, cancellationToken);

        if (professionalRequest == null)
            throw new DomainException(DomainErrors.ProfessionalRequest.NotFound);

        var message = new ProfessionalRequestInviteEmail(
            professionalRequest.User.Email,
            professionalRequest.Business.Name);

        await _notificationPublisher.PublishAsync(message, cancellationToken);
    }

    #endregion
}
