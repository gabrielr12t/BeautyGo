namespace BeautyGo.Contracts.Emails;

public class MailRequest
{
    public MailRequest(string emailTo, string subject, string body)
    {
        EmailTo = emailTo;
        Subject = subject;
        Body = body;
    }

    public  string EmailTo { get; set; }
    public  string Subject { get; set; }
    public  string Body { get; set; }
}
