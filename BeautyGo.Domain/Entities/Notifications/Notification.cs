using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Domain.Entities.Notifications;

public sealed class Notification : BaseEntity, IAuditableEntity, ISoftDeletableEntity
{
    public Guid UserId { get; private set; }

    public DateTime DateTimeUtc { get; private set; }

    public bool Sent { get; private set; }

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public DateTime? DeletedOnUtc { get; }

    public DateTime? Deleted { get; }

    public Result MarkAsSent()
    {
        if (Sent)
        {
            //return Result.Failure(DomainErrors.Notification.AlreadySent);
        }

        Sent = true;

        return Result.Success();
    }
}
