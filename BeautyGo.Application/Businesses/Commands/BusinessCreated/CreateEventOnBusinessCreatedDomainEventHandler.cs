using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Events;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

internal class CreateEventOnBusinessCreatedDomainEventHandler// : IDomainEventHandler<EntityInsertedEvent<Business>>
{
    private readonly IBaseRepository<Event> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    private DateTime _triggerTimeToValidateDocument => DateTime.Now.AddMinutes(1);

    public CreateEventOnBusinessCreatedDomainEventHandler(
        IBaseRepository<Event> eventRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task Handle(EntityInsertedEvent<Business> notification, CancellationToken cancellationToken)
    {
        //var currentUser = await _authService.GetCurrentUserAsync();

        //var newBusinessEvent = Event.Create(
        //    currentUser.Id,
        //    new BusinessCreatedEvent(notification.Entity.Id),
        //    _triggerTimeToValidateDocument);

        //await _eventRepository.InsertAsync(newBusinessEvent, cancellationToken);
        //await _unitOfWork.SaveChangesAsync(cancellationToken);

        await Task.CompletedTask;
    }
}
