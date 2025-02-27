namespace BeautyGo.Domain.Entities;

public sealed class OutboxMessage : BaseEntity
{
    public string Type { get; set; }

    public string Content { get; set; }

    public DateTime OcurredOn { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public string? Error { get; set; }
}
