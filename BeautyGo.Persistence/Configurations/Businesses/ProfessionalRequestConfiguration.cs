using BeautyGo.Domain.Entities.Professionals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Businesses;

public class ProfessionalRequestConfiguration : BaseEntityConfiguration<ProfessionalRequest>
{
    public override void Configure(EntityTypeBuilder<ProfessionalRequest> builder)
    {
        builder.ToTable("ProfessionalRequests", "Businesses");

        builder
            .HasOne(pi => pi.Business)
            .WithMany(b => b.ProfessionalRequests)
            .HasForeignKey(pi => pi.BusinessId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder
            .HasOne(pi => pi.User)
            .WithMany(b => b.ProfessionalInvitations)
            .HasForeignKey(pi => pi.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);

        base.Configure(builder);
    }
}
