using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents.EmailValidationToken;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Media;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;

namespace BeautyGo.Domain.Entities.Stores;

public class Store : BaseEntity, IAuditableEntity, ISoftDeletableEntity, IEmailValidationToken
{
    public string Name { get; set; }

    public string HomePageTitle { get; set; }

    public string HomePageDescription { get; set; }

    public string Url { get; set; }

    public string Description { get; set; }

    public string Cnpj { get; set; }

    public string Host { get; set; }

    public string Phone { get; set; }

    public DateTime? Deleted { get; set; }

    public bool IsActive { get; set; }

    public bool EmailConfirmed { get; set; }

    public Guid OwnerId { get; set; }
    public User Owner { get; set; }

    public Guid? AddressId { get; set; }
    public Address Address { get; set; }

    public ICollection<StorePicture> Pictures { get; set; }

    #region Methods

    public static Store Create(string name, string homePageTitle, string homePageDescription, string cnpj, Guid ownerId, Guid addressId)
    {
        var store = new Store
        {
            Name = name,
            HomePageTitle = homePageTitle,
            HomePageDescription = homePageDescription,
            Cnpj = CommonHelper.EnsureNumericOnly(cnpj),
            OwnerId = ownerId,
            IsActive = false,
            EmailConfirmed = false,
            AddressId = addressId
        };

        store.AddDomainEvent(new EntityEmailValidationTokenCreatedEvent(store));

        return store;
    }

    public void AddPicture(Picture picture) =>
        Pictures.Add(new StorePicture { StoreId = Id, PictureId = Id });

    public BeautyGoEmailTokenValidation CreateEmailValidationToken()
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new StoreEmailTokenValidation
        {
            CreatedOn = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(5),
            IsUsed = false,
            StoreId = Id,
            Token = token
        };
    }

    #endregion
}
