namespace BeautyGo.Domain.Entities.Outbox;

public class OutboxMessageError : BaseEntity
{
    public OutboxMessageError(string error, string stackTrace)
    {
        Error = error;
        StackTrace = stackTrace;
    }

    public string Error { get; set; }
    public string StackTrace { get; set; }

    public Guid OutboxMessageId { get; set; }
    public OutboxMessage OutboxMessage { get; set; }
}
