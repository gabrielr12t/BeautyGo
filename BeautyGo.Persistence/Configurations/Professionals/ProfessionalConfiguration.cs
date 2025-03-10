using BeautyGo.Domain.Entities.Professionals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Professionals;

public class ProfessionalConfiguration : BaseEntityConfiguration<Professional>
{
    public override void Configure(EntityTypeBuilder<Professional> builder)
    {
        //builder.ToTable("Professionals", "Users");

        builder.HasOne(p => p.Business)
            .WithMany(bb => bb.Professionals)
            .HasForeignKey(p => p.BusinessId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(p => p.Services)
            .WithOne(ps => ps.Professional)
            .HasForeignKey(ps => ps.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Professional)
            .HasForeignKey(a => a.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.ProfessionalAvailabilities)
            .WithOne(pa => pa.Professional)
            .HasForeignKey(pa => pa.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
