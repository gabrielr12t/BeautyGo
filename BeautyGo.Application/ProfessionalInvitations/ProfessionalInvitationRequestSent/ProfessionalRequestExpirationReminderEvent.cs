using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.ProfessionalInvitations.ProfessionalInvitationRequestSent;

public record ProfessionalRequestExpirationReminderEvent(
    Guid ProfessionalRequestId) : IEvent;
