﻿namespace BeautyGo.BackgroundTasks.Services.Emails;

internal interface IEmailNotificationsConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken = default);
}
