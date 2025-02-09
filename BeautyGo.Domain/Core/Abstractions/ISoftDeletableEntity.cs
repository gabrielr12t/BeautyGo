namespace BeautyGo.Domain.Core.Abstractions;

public interface ISoftDeletableEntity
{
    DateTime? Deleted { get; }
}
