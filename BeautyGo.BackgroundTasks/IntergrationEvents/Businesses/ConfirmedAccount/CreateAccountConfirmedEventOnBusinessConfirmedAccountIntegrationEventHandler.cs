﻿using BeautyGo.Application.Businesses.Commands.AccountConfirmed;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Events;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.ConfirmedAccount;

internal class CreateAccountConfirmedEventOnBusinessConfirmedAccountIntegrationEventHandler : IIntegrationEventHandler<BusinessAccountConfirmedIntegrationEvent>
{
    private DateTime _triggerToEvent => DateTime.Now.AddMinutes(1);

    private readonly IEFBaseRepository<Event> _eventRepository;
    private readonly IEFBaseRepository<Business> _businessRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountConfirmedEventOnBusinessConfirmedAccountIntegrationEventHandler(
        IEFBaseRepository<Event> eventRepository,
        IEFBaseRepository<Business> businessRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(BusinessAccountConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetFirstOrDefaultAsync(new EntityByIdSpecification<Business>(
                notification.BusinessId),
                cancellationToken: cancellationToken);

        if (business == null)
            throw new DomainException(DomainErrors.Business.BusinessNotFound(notification.BusinessId));

        var businessAccountConfirmedEvent = Event.Create(
            new BusinessAccountConfirmedEvent(business.Id),
            _triggerToEvent);

        await _eventRepository.InsertAsync(businessAccountConfirmedEvent, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
