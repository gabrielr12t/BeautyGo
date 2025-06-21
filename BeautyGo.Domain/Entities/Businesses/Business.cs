using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Media;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;

namespace BeautyGo.Domain.Entities.Businesses;

public class Business : BaseEntity, IAuditableEntity, ISoftDeletableEntity, IEmailValidationToken
{
    #region Ctor

    public Business()
    {
        Pictures = new List<BusinessPicture>();
        Professionals = new List<Professional>();
        WorkingHours = new List<BusinessWorkingHours>(7);
        ClosedDays = new List<BusinessClosedDay>();
        ValidationTokens = new List<BusinessEmailConfirmation>();
        ProfessionalRequests = new List<ProfessionalRequest>();
    }

    #endregion

    #region Properties

    public string Name { get; set; }

    public int Code { get; set; }

    public string HomePageTitle { get; set; }

    public string HomePageDescription { get; set; }

    public string Url { get; set; }

    public string Description { get; set; }

    public string Cnpj { get; set; }

    public bool DocumentValidated { get; set; }

    public string Host { get; set; }

    public string Phone { get; set; }

    public DateTime? Deleted { get; set; }

    public bool IsActive { get; set; }

    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public Guid OwnerId { get; set; }
    public BusinessOwner Owner { get; set; }

    public Guid AddressId { get; set; }
    public Address Address { get; set; }

    public ICollection<BusinessPicture> Pictures { get; set; }
    public ICollection<Professional> Professionals { get; set; }
    public ICollection<Service> Services { get; set; }
    public ICollection<BusinessWorkingHours> WorkingHours { get; set; }
    public ICollection<BusinessClosedDay> ClosedDays { get; set; }
    public ICollection<BusinessEmailConfirmation> ValidationTokens { get; set; }
    public ICollection<ProfessionalRequest> ProfessionalRequests { get; set; }

    #endregion

    #region Methods 

    public bool CanWork()
    {
        return IsActive && EmailConfirmed && !Deleted.HasValue && DocumentValidated;
    }

    public static Business Create(string name, string homePageTitle, string homePageDescription, string cnpj, Guid ownerId, Guid addressId)
    {
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = name,
            HomePageTitle = homePageTitle,
            HomePageDescription = homePageDescription,
            Cnpj = CommonHelper.EnsureNumericOnly(cnpj),
            OwnerId = ownerId,
            IsActive = false,
            EmailConfirmed = false,
            AddressId = addressId,
            DocumentValidated = false,
        };

        return business;
    }

    public void SendProfessionalRequest(User user)
    {
        var professionalRequest = ProfessionalRequest.Create(this, user);

        ProfessionalRequests.Add(professionalRequest);

        AddDomainEvent(new ProfessionalRequestSentDomainEvent(professionalRequest));
    }

    public void ConfirmAccount()
    {
        EmailConfirmed = true;
        AddDomainEvent(new BusinessAccountConfirmedDomainEvent(this));
    }

    public void ValidatedDocument()
    {
        DocumentValidated = true;
        IsActive = true;
        AddDomainEvent(new BusinessDocumentValidatedDomainEvent(this));
    }

    public void ClearWorkingHours()
    {
        WorkingHours.Clear();
    }

    public void AddProfessional(Professional professional)
    {
        Professionals.Add(professional);

        //AddDomainEvent(new BusinessProfessionalAddedDomainEvent(professional));
    }

    public void AddWorkingHours(IEnumerable<BusinessWorkingHours> workingHours) =>
        workingHours.ToList().ForEach(AddWorkingHours);

    public void AddWorkingHours(BusinessWorkingHours workingHours) =>
         WorkingHours.Add(workingHours);

    public bool HasWorkingHours() =>
        WorkingHours.Any();

    public void AddPicture(Picture picture) =>
        Pictures.Add(new BusinessPicture { BeautyBusinessId = Id, PictureId = Id });

    public void AddValidationToken() =>
        ValidationTokens.Add(BusinessEmailConfirmation.Create(Id));

    public bool IsOwner(User user) =>
        user != null &&
        user is BusinessOwner &&
        OwnerId == user.Id;

    public EmailConfirmation CreateEmailValidationToken()
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new BusinessEmailConfirmation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(10),
            BusinessId = Id,
            IsUsed = false
        };
    }

    #endregion
}
