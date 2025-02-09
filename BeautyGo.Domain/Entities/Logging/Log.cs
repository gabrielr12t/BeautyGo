using BeautyGo.Domain.Core.Abstractions;

namespace BeautyGo.Domain.Entities.Logging;

public class Log : BaseEntity, ISoftDeletableEntity
{
    public int LogLevelId { get; set; }

    public string ShortMessage { get; set; }

    public string FullMessage { get; set; }

    public string IpAddress { get; set; }

    public Guid? UserId { get; set; }

    public string PageUrl { get; set; }

    public string ReferrerUrl { get; set; } 

    public DateTime? Deleted { get; set; }

    public LogLevel LogLevel
    {
        get => (LogLevel)LogLevelId;
        set => LogLevelId = (int)value;
    }
}
