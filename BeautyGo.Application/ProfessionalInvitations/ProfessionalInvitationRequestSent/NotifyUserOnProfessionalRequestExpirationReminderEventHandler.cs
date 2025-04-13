using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.ProfessionalInvitations.ProfessionalInvitationRequestSent;

public class NotifyUserOnProfessionalRequestExpirationReminderEventHandler : IEventHandler<ProfessionalRequestExpirationReminderEvent>
{
    private readonly IBaseRepository<ProfessionalRequest> _professionalRequestRepository;

    public NotifyUserOnProfessionalRequestExpirationReminderEventHandler(
        IBaseRepository<ProfessionalRequest> professionalRequestRepository)
    {
        _professionalRequestRepository = professionalRequestRepository;
    }

    public async Task Handle(ProfessionalRequestExpirationReminderEvent notification, CancellationToken cancellationToken)
    {
        var professionalRequest = await _professionalRequestRepository.GetByIdAsync(notification.ProfessionalRequestId, cancellationToken);

        if (professionalRequest == null)
            throw new DomainException(DomainErrors.General.EntityNotFound(notification.ProfessionalRequestId));

        if (professionalRequest.Status != InvitationStatus.Pending)
            return;

        //SEND NOTIFICATIONS
        //ENVIAR EMAIL PARA USUÁRIO DESTINDO PROFESSIONAL REQUEST PARA LEMBRAR SOBRE O CONVITE E QUE ESTÁ EXPIRANDO.
    }
}
