namespace BeautyGo.BackgroundTasks;

public interface IRabbitMqRetryPolicy
{
    Task ExecuteAsync(Func<Task> action);
}
