﻿namespace BeautyGo.Application.Core.Abstractions.Messaging;

public interface IPublisherBusEvent
{
    Task PublishAsync(IBusEvent @event, CancellationToken cancellationToken = default);  
}
