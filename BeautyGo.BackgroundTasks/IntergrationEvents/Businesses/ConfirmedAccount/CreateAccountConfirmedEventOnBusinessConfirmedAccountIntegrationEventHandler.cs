using BeautyGo.Application.Businesses.Commands.AccountConfirmed;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Events;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.ConfirmedAccount;

internal class CreateAccountConfirmedEventOnBusinessConfirmedAccountIntegrationEventHandler : IIntegrationEventHandler<BusinessAccountConfirmedIntegrationEvent>
{
    private DateTime _triggerToEvent => DateTime.Now.AddMinutes(1);

    private readonly IBaseRepository<Event> _eventRepository;
    private readonly IBaseRepository<Business> _businessRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountConfirmedEventOnBusinessConfirmedAccountIntegrationEventHandler(
        IBaseRepository<Event> eventRepository,
        IBaseRepository<Business> businessRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(BusinessAccountConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetFirstOrDefaultAsync(new EntityByIdSpecification<Business>(notification.BusinessId));

        if (business == null)
            throw new DomainException(DomainErrors.Business.BusinessNotFound(notification.BusinessId));

        var businessAccountConfirmedEvent = Event.Create(
            business.OwnerId,
            new BusinessAccountConfirmedEvent(business.Id),
            _triggerToEvent);

        await _eventRepository.InsertAsync(businessAccountConfirmedEvent, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
