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

    public static EmailNotification Create(string recipientEmail, string subject, string body, DateTime scheduleDate, bool isSent = false)
    {
        return new EmailNotification
        {
            Subject = subject,
            Body = body,
            ScheduledDate = scheduleDate,
            IsSent = isSent,
            RecipientEmail = recipientEmail
        };
    }
}
