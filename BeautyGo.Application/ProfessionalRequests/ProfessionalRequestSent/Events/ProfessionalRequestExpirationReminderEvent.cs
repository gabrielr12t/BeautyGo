using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent.Events;

public record ProfessionalRequestExpirationReminderEvent(Guid ProfessionalRequestId) : IEvent;
