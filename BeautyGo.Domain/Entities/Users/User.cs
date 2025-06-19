using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents.Users;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Security;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Users;

public abstract class User : BaseEntity, IAuditableEntity, IEmailValidationToken
{
    #region Ctor

    public User()
    {
        UserRoles = new HashSet<UserRoleMapping>();
        Passwords = new List<UserPassword>();
        Addresses = new List<UserAddressMapping>();
        ValidationTokens = new List<UserEmailConfirmation>();
        ProfessionalInvitations = new List<ProfessionalRequest>();
    }

    public User(string firstName, string lastName, string email, string phoneNumber, string cpf) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Cpf = cpf;
    }

    #endregion

    #region Properties

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string EmailToRevalidate { get; set; }

    public DateTime? CannotLoginUntilDate { get; set; }

    public string LastIpAddress { get; private set; }

    public DateTime LastLoginDate { get; set; }

    public DateTime LastActivityDate { get; set; }

    public bool EmailConfirmed { get; set; }

    public string Cpf { get; set; }

    public string PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public ICollection<UserRoleMapping> UserRoles { get; set; }
    public ICollection<UserAddressMapping> Addresses { get; set; }
    public ICollection<UserPassword> Passwords { get; set; }
    public ICollection<UserEmailConfirmation> ValidationTokens { get; set; }
    public ICollection<ProfessionalRequest> ProfessionalInvitations { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }

    #endregion

    #region Methods

    #region Promote User

    private T PromoteTo<T>(Guid? businessId = null) where T : User, IUserPromotable, new()
    {
        var user = new T
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Cpf = Cpf,
            Id = Id,
            EmailConfirmed = EmailConfirmed,
            EmailToRevalidate = EmailToRevalidate,
            CannotLoginUntilDate = CannotLoginUntilDate,
            IsActive = IsActive,
            LastActivityDate = LastActivityDate,
            LastLoginDate = LastLoginDate,
            MustChangePassword = MustChangePassword,
            Passwords = Passwords,
            Addresses = Addresses,
            UserRoles = UserRoles,
            ValidationTokens = ValidationTokens,
            CreatedOn = CreatedOn,
            DateOfBirth = DateOfBirth
        };

        user.PromoteSpecificProperties(businessId);

        return user;
    }

    public BusinessOwner PromoteToOwner() =>
        PromoteTo<BusinessOwner>();

    public Professional PromoteToProfessional(Guid businessId) =>
        PromoteTo<Professional>(businessId);

    #endregion 

    public abstract Task HandleUserRoleAccept(IUserRoleHandlerVisitor visitor);

    public RefreshToken GetValidRefreshToken()
    {
        return RefreshTokens
            .SingleOrDefault(p => p.IsValid());
    }

    public void ActivateUser()
    {
        IsActive = true;
    }

    public void ChangeIpAddress(string ipAddress)
    {
        if (ipAddress != LastIpAddress)
            AddDomainEvent(new UserIpAddressChangedDomainEvent(this));

        LastIpAddress = ipAddress;
    }

    public void ChangeName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        AddDomainEvent(new UserNameChangedDomainEvent(this));
    }

    public void ConfirmAccount()
    {
        EmailConfirmed = true;
        IsActive = true;
        LastActivityDate = DateTime.Now;
        AddDomainEvent(new UserConfirmedAccountDomainEvent(this));
    }

    public string FullName() =>
        $"{FirstName} {LastName}";

    public UserPassword GetCurrentPassword() =>
        Passwords.MaxBy(p => p.CreatedOn);

    public bool HasRole(UserRole role) =>
        UserRoles.Any(p => p.UserRole == role);

    public void AddUserRole(UserRole userRole) =>
        UserRoles.Add(UserRoleMapping.Create(userRole, this));

    public void AddUserPassword(string password, string salt) =>
        Passwords.Add(UserPassword.Create(password, salt));

    public void AddValidationToken() =>
        ValidationTokens.Add(UserEmailConfirmation.Create(Id));

    public EmailConfirmation CreateEmailValidationToken()
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new UserEmailConfirmation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddMinutes(10),
            UserId = Id,
            IsUsed = false
        };
    }

    #endregion
}

