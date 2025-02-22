using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents.EmailValidationToken;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Media;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;

namespace BeautyGo.Domain.Entities.Business;

public class BeautyBusiness : BaseEntity, IAuditableEntity, ISoftDeletableEntity, IEmailValidationToken
{
    public BeautyBusiness()
    {
        Pictures = new List<BeautyBusinessPicture>();
        Professionals = new List<Professional>();
        BusinessWorkingHours = new List<BusinessWorkingHours>();
        ValidationTokens = new List<BeautyBusinessEmailTokenValidation>();
    }

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

    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public bool Processed { get; set; }

    public Guid CreatedId { get; set; }
    public User Created { get; set; }

    public Guid AddressId { get; set; }
    public Address Address { get; set; }

    public ICollection<BeautyBusinessPicture> Pictures { get; set; }
    public ICollection<Professional> Professionals { get; set; }
    public ICollection<Service> Services { get; set; }
    public ICollection<BusinessWorkingHours> BusinessWorkingHours { get; set; }
    public ICollection<BeautyBusinessEmailTokenValidation> ValidationTokens { get; set; }

    #region Methods

    public static BeautyBusiness Create(string name, string homePageTitle, string homePageDescription, string cnpj, Guid ownerId, Guid addressId)
    {
        var business = new BeautyBusiness
        {
            Name = name,
            HomePageTitle = homePageTitle,
            HomePageDescription = homePageDescription,
            Cnpj = CommonHelper.EnsureNumericOnly(cnpj),
            CreatedId = ownerId,
            IsActive = false,
            EmailConfirmed = false,
            AddressId = addressId,
            Processed = false
        };

        //business.AddDomainEvent(new EntityEmailValidationTokenCreatedEvent(business));

        return business;
    } 

    public void AddWorkingHours(IEnumerable<BusinessWorkingHours> workingHours)
    {
        foreach (var workingHour in workingHours)
        {
            AddWorkingHours(workingHour);
        }
    }

    public void AddWorkingHours(BusinessWorkingHours workingHours)
    {
        BusinessWorkingHours.Add(workingHours);
    }

    public void AddPicture(Picture picture) =>
        Pictures.Add(new BeautyBusinessPicture { BeautyBusinessId = Id, PictureId = Id });

    public EmailTokenValidation CreateEmailValidationToken()
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
