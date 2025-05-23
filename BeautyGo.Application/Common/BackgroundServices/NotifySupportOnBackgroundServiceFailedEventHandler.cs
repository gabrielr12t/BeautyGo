using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Common.BackgroundServices;

public class NotifySupportOnBackgroundServiceFailedEventHandler :
    IEventHandler<BusEventConsumerFailedEvent>,
    IEventHandler<EmailNotificationFailedEvent>,
    IEventHandler<EventProcessorFailedEvent>,
    IEventHandler<ProcessOuboxMessageFailedEvent>
{
    private readonly ISupportNotificationPublisher _notificationPublisher;

    public NotifySupportOnBackgroundServiceFailedEventHandler(
        ISupportNotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(BusEventConsumerFailedEvent notification, CancellationToken cancellationToken)
    {
        var body = $"Ocorreu um erro no BackgroundService 'BusEventConsumer'" +
            Environment.NewLine +
            Environment.NewLine +
            $"Evento: {notification.Event}" +
            Environment.NewLine +
            Environment.NewLine +
            $"Erro: {notification.Error}";

        var subject = $"Erro em BusEventConsumer";

        await _notificationPublisher.PublishAsync(new SupportBackgroundFailedEmail(subject, body), cancellationToken);
    }

    public async Task Handle(EmailNotificationFailedEvent notification, CancellationToken cancellationToken)
    {
        var body = $"Ocorreu um erro no BackgroundService 'EmailNotification'" +
            Environment.NewLine +
            Environment.NewLine +
            $"EmailId: {notification.EmailId}" +
            Environment.NewLine +
            Environment.NewLine +
            $"Erro: {notification.Error}";

        var subject = $"Erro em EmailNotification";

        await _notificationPublisher.PublishAsync(new SupportBackgroundFailedEmail(subject, body), cancellationToken);
    }

    public async Task Handle(EventProcessorFailedEvent notification, CancellationToken cancellationToken)
    {
        var body = $"Ocorreu um erro no BackgroundService 'EventProcessor'" +
             Environment.NewLine +
             Environment.NewLine +
             $"EventId: {notification.EventId}" +
             Environment.NewLine +
             Environment.NewLine +
             $"Erro: {notification.Error}";

        var subject = $"Erro em EventProcessor";

        await _notificationPublisher.PublishAsync(new SupportBackgroundFailedEmail(subject, body), cancellationToken);
    }

    public async Task Handle(ProcessOuboxMessageFailedEvent notification, CancellationToken cancellationToken)
    {
        var body = $"Ocorreu um erro no BackgroundService 'ProcessOuboxMessage'" +
             Environment.NewLine +
             Environment.NewLine +
             $"MessageId: {notification.MessageId}" +
             Environment.NewLine +
             Environment.NewLine +
             $"Erro: {notification.Error}";

        var subject = $"Erro em ProcessOuboxMessage";

        await _notificationPublisher.PublishAsync(new SupportBackgroundFailedEmail(subject, body), cancellationToken);
    }
}
