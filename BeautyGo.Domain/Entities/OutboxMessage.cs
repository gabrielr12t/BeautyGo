namespace BeautyGo.Domain.Entities;

public sealed class OutboxMessage : BaseEntity
{
    public string Type { get; set; }

    public string Content { get; set; }

    public DateTime OccurredOn { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public string? Error { get; set; }

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
