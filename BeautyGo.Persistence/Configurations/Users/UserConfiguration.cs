using BeautyGo.Domain.Entities.Customers;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Users;

internal class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        //builder.UseTptMappingStrategy();

        builder.UseTphMappingStrategy()
            .ToTable("User", "Users")
            .HasDiscriminator<string>("UserType")
            .HasValue<Customer>(nameof(Customer))
            .HasValue<Professional>(nameof(Professional));

        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(u => u.DateOfBirth)
            .IsRequired(false);

        builder.Property(u => u.EmailToRevalidate)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(u => u.LastIpAddress)
            .HasMaxLength(45)  // Para armazenar IPv6
            .IsRequired(false);

        builder.Property(u => u.LastLoginDate)
            .IsRequired();

        builder.Property(u => u.LastActivityDate)
            .IsRequired();

        builder.Property(u => u.Cpf)
            .HasMaxLength(11)
            .IsRequired(false);

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.MustChangePassword)
            .IsRequired();

        //builder.HasMany(u => u.ShoppingCartItems)
        //    .WithOne()
        //    .HasForeignKey(sci => sci.CustomerId)
        //    .OnDelete(DeleteBehavior.Cascade);   

        //builder.HasMany(u => u.UserRoles)
        //    .WithOne()
        //    .HasForeignKey(uurm => uurm.UserId)
        //    .OnDelete(DeleteBehavior.Cascade);   

        //builder.HasMany(u => u.Addresses)
        //    .WithOne()
        //    .HasForeignKey(uam => uam.UserId)
        //    .OnDelete(DeleteBehavior.SetNull);  

        //builder.HasMany(u => u.Stores)
        //    .WithOne()
        //    .HasForeignKey(s => s.OwnerId)
        //    .OnDelete(DeleteBehavior.Cascade);   

        //builder.HasMany(u => u.Orders)
        //    .WithOne()
        //    .HasForeignKey(o => o.CustomerId)
        //    .OnDelete(DeleteBehavior.Cascade);   

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Cpf)
            .IsUnique();

        builder.HasIndex(u => u.LastLoginDate);
    }
}
