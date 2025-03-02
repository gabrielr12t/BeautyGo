using BeautyGo.Domain.Entities.Businesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Businesses;

internal class BusinessConfiguration : BaseEntityConfiguration<Business>
{
    public override void Configure(EntityTypeBuilder<Business> builder)
    {
        base.Configure(builder);

        builder.ToTable("Business", "Businesses");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Code)
           .HasDefaultValueSql("NEXT VALUE FOR CodeSequence");

        builder.HasIndex(p => p.Code)
            .HasDatabaseName("IX_CODE");

        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_NAME")
            .IsUnique();

        builder.Property(s => s.HomePageTitle)
            .HasMaxLength(200);

        builder.Property(s => s.HomePageDescription)
            .HasMaxLength(500);

        builder.Property(s => s.Url)
            .HasMaxLength(500);

        builder.HasIndex(p => p.Url)
           .HasDatabaseName("IX_URL")
           .IsUnique();

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Cnpj)
            .HasMaxLength(20);

        builder.HasIndex(p => p.Cnpj)
            .HasDatabaseName("IX_CNPJ")
            .IsUnique();

        builder.Property(s => s.Host)
            .HasMaxLength(100);

        builder.Property(s => s.Phone)
            .HasMaxLength(20);

        builder.HasIndex(p => p.Phone)
            .HasDatabaseName("IX_PHONE")
           .IsUnique();

        //builder.Property(s => s.Code)
        //    .IsRequired()
        //    .ValueGeneratedOnAdd();

        //builder.HasIndex(p => p.Code)
        //   .IsUnique();

        builder.Property(s => s.Deleted);

        builder.HasOne(s => s.Created)
            .WithMany()
            .HasForeignKey(s => s.CreatedId)
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
