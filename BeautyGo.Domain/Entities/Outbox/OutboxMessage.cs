namespace BeautyGo.Domain.Entities.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public string Type { get; set; }

    public string Content { get; set; }

    public DateTime OccurredOn { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public ICollection<OutboxMessageError> Errors { get; set; } = [];

    public int Attempts { get; set; }

    public static OutboxMessage Create(string type, string content)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOn = DateTime.Now,
            Type = type,
            Content = content
        };
    }
}
