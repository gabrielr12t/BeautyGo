namespace BeautyGo.BackgroundTasks.Services.Emails;

internal interface IEmailNotificationsConsumer
{
    Task ConsumeAsync(int batchSize, CancellationToken cancellationToken = default);
}
