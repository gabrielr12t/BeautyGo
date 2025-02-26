namespace BeautyGo.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string Content { get; set; }

    public DateTime OcurredOn { get; set; }

    public DateTime ProcessedOn { get; set; }

    public string? Error { get; set; }
}
