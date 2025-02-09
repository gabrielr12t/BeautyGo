using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents.EmailValidationToken;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Media;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;

namespace BeautyGo.Domain.Entities.Business;

public class BeautyBusiness : BaseEntity, IAuditableEntity, ISoftDeletableEntity, IEmailValidationToken
{
    public string Name { get; set; }

    public int Code { get; set; }

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

    public ICollection<BeautyBusinessPicture> Pictures { get; set; }

    #region Methods

    public static BeautyBusiness Create(string name, string homePageTitle, string homePageDescription, string cnpj, Guid ownerId, Guid addressId)
    {
        var business = new BeautyBusiness
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

        business.AddDomainEvent(new EntityEmailValidationTokenCreatedEvent(business));

        return business;
    }

    public void AddPicture(Picture picture) =>
        Pictures.Add(new BeautyBusinessPicture { BeautyBusinessId = Id, PictureId = Id });

    public BeautyGoEmailTokenValidation CreateEmailValidationToken()
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new BeautyBusinessEmailTokenValidation
        {
            CreatedOn = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(5),
            IsUsed = false,
            BusinessId = Id,
            Token = token
        };
    }

    #endregion
}
