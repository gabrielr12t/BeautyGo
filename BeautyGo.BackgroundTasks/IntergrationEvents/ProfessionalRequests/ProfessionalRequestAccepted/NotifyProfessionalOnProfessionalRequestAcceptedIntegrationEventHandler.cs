﻿using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.ProfessionalRequests.ProfessionalRequestAccepted;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestAccepted;

internal class NotifyProfessionalOnProfessionalRequestAcceptedIntegrationEventHandler : IIntegrationEventHandler<ProfessionalRequestAcceptedIntegrationEvent>
{
    #region Fields

    private readonly IEFBaseRepository<ProfessionalRequest> _professionalRequestRepository;
    private readonly IProfessionalEmailNotificationPublisher _notificationPublisher;

    #endregion

    #region Ctor

    public NotifyProfessionalOnProfessionalRequestAcceptedIntegrationEventHandler(
        IEFBaseRepository<ProfessionalRequest> professionalRequestRepository,
        IProfessionalEmailNotificationPublisher notificationPublisher)
    {
        _professionalRequestRepository = professionalRequestRepository;
        _notificationPublisher = notificationPublisher;
    }

    #endregion

    #region Utilities

    private async Task<ProfessionalRequest> GetProfessionalRequestByIdAsync(Guid professionalRequestId, CancellationToken cancellationToken)
    {
        var professionalRequestByIdSpec = new EntityByIdSpecification<ProfessionalRequest>(professionalRequestId)
            .AddInclude(
                p => p.Include(i => i.Business),
                p => p.Include(i => i.User)
            );

        return await _professionalRequestRepository.GetFirstOrDefaultAsync(professionalRequestByIdSpec, false, cancellationToken);
    }

    #endregion

    #region Handle

    public async Task Handle(ProfessionalRequestAcceptedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var professionalRequest = await GetProfessionalRequestByIdAsync(notification.ProfessionalRequestId, cancellationToken);

        if (professionalRequest == null)
            throw new DomainException(DomainErrors.ProfessionalRequest.NotFound);

        //VALIDAR OS OBJETOS (BUSINESS e USER) DENTRO DE 'professionalRequest'

        var message = new BusinessProfessionalAddedEmail(
            professionalRequest.Business.Email,
            professionalRequest.User.FullName(),
            professionalRequest.Business.Name);

        await _notificationPublisher.PublishAsync(message, cancellationToken);
    }

    #endregion 
}
