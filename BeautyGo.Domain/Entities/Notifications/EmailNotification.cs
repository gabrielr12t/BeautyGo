namespace BeautyGo.Domain.Entities.Notifications;

public class EmailNotification : BaseEntity
{
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime ScheduledDate { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentDate { get; set; }
    public int RetryCount { get; set; }
    public DateTime? FailedDate { get; set; }
    public string ErrorMessage { get; set; }
}
