using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent.Events;

public class NotifyUserOnProfessionalRequestExpirationReminderEventHandler : IEventHandler<ProfessionalRequestExpirationReminderEvent>
{
    private readonly IBaseRepository<ProfessionalRequest> _professionalRequestRepository;
    private readonly IProfessionalRequestEmailNotificationPublisher _notification;

    public NotifyUserOnProfessionalRequestExpirationReminderEventHandler(
        IBaseRepository<ProfessionalRequest> professionalRequestRepository,
        IProfessionalRequestEmailNotificationPublisher professionalRequestEmailNotificationPublisher)
    {
        _professionalRequestRepository = professionalRequestRepository;
        _notification = professionalRequestEmailNotificationPublisher;
    }

    #region Utilities

    private async Task<ProfessionalRequest> GetProfessionalRequestAsync(Guid professionalRequestId, CancellationToken cancellationToken)
    {
        var professionalRequestByIdSpec = new EntityByIdSpecification<ProfessionalRequest>(professionalRequestId)
            .AddInclude(p => p.User)
            .AddInclude(p => p.Business);

        return await _professionalRequestRepository.GetFirstOrDefaultAsync(professionalRequestByIdSpec, cancellationToken: cancellationToken);
    }

    #endregion

    public async Task Handle(ProfessionalRequestExpirationReminderEvent notification, CancellationToken cancellationToken)
    {
        var professionalRequest = await GetProfessionalRequestAsync(notification.ProfessionalRequestId, cancellationToken);

        if (professionalRequest == null)
            throw new DomainException(DomainErrors.General.EntityNotFound(notification.ProfessionalRequestId));

        if (professionalRequest.Status != InvitationStatus.Pending)
            return;

        if (professionalRequest.IsExpired())
            return;

        var message = new ProfessionalRequestReminderEmail(
            professionalRequest.User.Email,
            professionalRequest.User.FullName(),
            professionalRequest.Business.Name,
            professionalRequest.ExpireAt);

        await _notification.PublishAsync(message, cancellationToken);
    }
}
