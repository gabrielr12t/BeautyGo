using BeautyGo.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Notifications
{
    internal class EmailNotificationConfiguration : BaseEntityConfiguration<EmailNotification>
    {
        public override void Configure(EntityTypeBuilder<EmailNotification> builder)
        {
            base.Configure(builder);

            builder.ToTable("Emails", "Notifications");

            builder.Property(n => n.RecipientEmail)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Subject)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Body)
                .IsRequired();

            builder.Property(n => n.ScheduledDate)
                .IsRequired();

            builder.Property(n => n.IsSent)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(n => n.RetryCount)
                .IsRequired()
                .HasDefaultValue(0);  

            builder.Property(n => n.FailedDate)
                .IsRequired(false);  

            builder.Property(n => n.ErrorMessage)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.HasIndex(n => n.ScheduledDate);
        }
    }
}
