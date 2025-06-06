﻿using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent;
using BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent.Events;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Events;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestSent;

//Criar JOB para notificar o usuário um dia antes de expirar o prazo.
//Excluir job caso o usuário aceite ou recuse o convite
internal class CreateProfessionalRequestExpirationReminderOnProfessionalRequestSentIntegrationEvent
    : IIntegrationEventHandler<ProfessionalRequestSentIntegrationEvent>
{
    #region Fields

    private readonly IEFBaseRepository<ProfessionalRequest> _professionalRequestRepository;
    private readonly IEFBaseRepository<Event> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public CreateProfessionalRequestExpirationReminderOnProfessionalRequestSentIntegrationEvent(
        IEFBaseRepository<ProfessionalRequest> professionalRequestRepository,
        IEFBaseRepository<Event> eventRepository,
        IUnitOfWork unitOfWork)
    {
        _professionalRequestRepository = professionalRequestRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task Handle(ProfessionalRequestSentIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var professionalRequest = await _professionalRequestRepository.GetByIdAsync(notification.ProfessionalRequestId, cancellationToken);

        if (professionalRequest is null)
            throw new DomainException(DomainErrors.General.EntityNotFound(notification.ProfessionalRequestId));

        var notificationAt = professionalRequest.ExpireAt.Date.AddDays(-1).AddHours(8);

        var professionalRequestExpirationReminderEvent = Event.Create(
            new ProfessionalRequestExpirationReminderEvent(notification.ProfessionalRequestId),
        notificationAt);

        await _eventRepository.InsertAsync(professionalRequestExpirationReminderEvent, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
