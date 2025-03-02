using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

public class BusinessCreatedEvent : IEvent 
{
    public BusinessCreatedEvent(Guid beautyBusinessId)
    {
        BeautyBusinessId = beautyBusinessId;
    }

    public Guid BeautyBusinessId{ get; set; }
}

