﻿using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.Users.Events.EmailConfirmed;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Users.UserCreated;

internal class SendWelcomeEmailOnUserConfirmEmaildIntegrationEventHandler : IIntegrationEventHandler<UserConfirmedEmailIntegrationEvent>
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    public SendWelcomeEmailOnUserConfirmEmaildIntegrationEventHandler(
        IBaseRepository<User> userRepository,
        IEmailNotificationService emailNotificationService)
    {
        _userRepository = userRepository;
        _emailNotificationService = emailNotificationService;
    }

    public async Task Handle(UserConfirmedEmailIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var userByIdSpecification = new EntityByIdSpecification<User>(notification.UserId);
        var user = await _userRepository.GetFirstOrDefaultAsync(userByIdSpecification);

        var message = new WelcomeEmail(user.Email, user.FullName());

        await _emailNotificationService.SendAsync(message);
    }
}
