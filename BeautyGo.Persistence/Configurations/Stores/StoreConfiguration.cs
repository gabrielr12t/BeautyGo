using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Stores;

internal class StoreConfiguration : BaseEntityConfiguration<Store>
{
    public override void Configure(EntityTypeBuilder<Store> builder)
    {
        base.Configure(builder);

        builder.ToTable("Stores", "Store");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.Property(s => s.HomePageTitle)
            .HasMaxLength(200);

        builder.Property(s => s.HomePageDescription)
            .HasMaxLength(500);

        builder.Property(s => s.Url)
            .HasMaxLength(500);

        builder.HasIndex(p => p.Url)
           .IsUnique();

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Cnpj)
            .HasMaxLength(20);

        builder.HasIndex(p => p.Cnpj)
            .IsUnique();

        builder.Property(s => s.Host)
            .HasMaxLength(100);

        builder.Property(s => s.Phone)
            .HasMaxLength(20);

        builder.HasIndex(p => p.Phone)
           .IsUnique();

        //builder.Property(s => s.Code)
        //    .IsRequired()
        //    .ValueGeneratedOnAdd();

        //builder.HasIndex(p => p.Code)
        //   .IsUnique();

        builder.Property(s => s.Deleted)
            .IsRequired(false);

        builder.Property(s => s.Deleted)
            .IsRequired();

        builder.HasOne(s => s.Owner)
            .WithMany()
            .HasForeignKey(s => s.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Address)
            .WithMany()
            .HasForeignKey(s => s.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.HasMany(s => s.Products)
        //    .WithOne()
        //    .HasForeignKey(p => p.StoreId)
        //    .OnDelete(DeleteBehavior.Cascade); 

        //builder.HasMany(s => s.Orders)
        //    .WithOne()
        //    .HasForeignKey(o => o.StoreId)
        //    .OnDelete(DeleteBehavior.Cascade); 

        //builder.HasMany(s => s.Pictures)
        //    .WithOne()
        //    .HasForeignKey(sp => sp.StoreId)
        //    .OnDelete(DeleteBehavior.Cascade); 
    }
}
