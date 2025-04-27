using BeautyGo.Domain.Entities.Notifications;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Notifications
{
    public class PendingEmailNotificationsSpecification : Specification<EmailNotification>
    {
        private readonly DateTime _date;

        public PendingEmailNotificationsSpecification(DateTime date)
        {
            _date = date;
        }

        public override Expression<Func<EmailNotification, bool>> ToExpression() =>
            notification => !notification.IsSent
                            && notification.RetryCount <= 3
                            && (notification.FailedDate == null || notification.ScheduledDate <= _date);
    }
}
