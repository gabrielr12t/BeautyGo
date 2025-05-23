﻿using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.Users.Commands.AccountConfirmed;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Users.ConfirmedAccount;

internal class SendWelcomeEmailOnUserConfirmedAccountEmaildIntegrationEventHandler
    : IIntegrationEventHandler<UserConfirmedAccountIntegrationEvent>
{
    private readonly IEFBaseRepository<User> _userRepository;
    private readonly IUserEmailNotificationPublisher _emailNotificationPublisher;

    public SendWelcomeEmailOnUserConfirmedAccountEmaildIntegrationEventHandler(
        IEFBaseRepository<User> userRepository,
        IUserEmailNotificationPublisher emailNotificationPublisher)
    {
        _userRepository = userRepository;
        _emailNotificationPublisher = emailNotificationPublisher;
    }

    public async Task Handle(UserConfirmedAccountIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var userByIdSpecification = new EntityByIdSpecification<User>(notification.UserId);
        var user = await _userRepository.GetFirstOrDefaultAsync(userByIdSpecification, false, cancellationToken);

        var message = new WelcomeEmail(user.Email, user.FullName());

        await _emailNotificationPublisher.PublishAsync(message, cancellationToken);
    }
}
