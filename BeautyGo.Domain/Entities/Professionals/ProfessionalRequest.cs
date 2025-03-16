﻿using BeautyGo.Domain.DomainEvents.Professionals;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.Entities.Professionals;

public class ProfessionalRequest : BaseEntity
{
    public Guid BusinessId { get; set; }
    public Business Business { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public InvitationStatus Status { get; set; }

    public DateTime SentDate { get; set; }
    public DateTime? AcceptedDate { get; set; }

    public static ProfessionalRequest Create(Business business, User user)
    {
        var professionalInvitation = new ProfessionalRequest
        {
            Business = business,
            User = user,
            AcceptedDate = null,
            SentDate = DateTime.Now,
            Status = InvitationStatus.Pending
        };

        professionalInvitation.AddDomainEvent(new ProfessionalRequestSentDomainEvent(professionalInvitation));

        return professionalInvitation;
    }
}
