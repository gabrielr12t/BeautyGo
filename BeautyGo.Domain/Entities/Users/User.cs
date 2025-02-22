using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Users;

public abstract class User : BaseEntity, IAuditableEntity, IEmailValidationToken
{
    public User()
    {
        UserRoles = new HashSet<UserRoleMapping>();
        Passwords = new List<UserPassword>();
        Addresses = new List<UserAddressMapping>();
        ValidationTokens = new List<UserEmailTokenValidation>();

        AddDomainEvent(new UserCreatedDomainEvent(this));
    }

    public User(string firstName, string lastName, string email, string phoneNumber) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string EmailToRevalidate { get; set; }

    public DateTime? CannotLoginUntilDate { get; set; }

    public string LastIpAddress { get; set; }

    public DateTime LastLoginDate { get; set; }

    public DateTime LastActivityDate { get; set; }

    public bool EmailConfirmed { get; set; }

    public string Cpf { get; set; }

    public string PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public Guid? BillingAddressId { get; set; }

    public Guid? ShippingAddressId { get; set; }

    public ICollection<UserRoleMapping> UserRoles { get; set; }

    public ICollection<UserAddressMapping> Addresses { get; set; }

    public ICollection<UserPassword> Passwords { get; set; }
    public ICollection<UserEmailTokenValidation> ValidationTokens { get; set; }

    #region Methods

    public abstract Task HandleUserRoleAccept(IUserRoleHandlerVisitor visitor);

    public void ActivateUser()
    {
        IsActive = true;
    }

    public void ChangeName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        AddDomainEvent(new UserNameChangedDomainEvent(this));
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        IsActive = true;
        AddDomainEvent(new UserConfirmEmailDomainEvent(this));
    }

    public string FullName() =>
        $"{FirstName} {LastName}";

    public UserPassword GetCurrentPassword() =>
        Passwords.MaxBy(p => p.CreatedOn);

    public bool HasRole(UserRole role) =>
        UserRoles.Any(p => p.UserRole == role);

    public void AddUserRole(UserRole userRole) =>
        UserRoles.Add(new UserRoleMapping { UserRoleId = userRole.Id, User = this });

    public void AddUserPassword(string password, string salt) =>
        Passwords.Add(new UserPassword { User = this, Password = password, Salt = salt });

    public void AddValidationToken() =>
        ValidationTokens.Add(UserEmailTokenValidation.Create(Id));

    public EmailTokenValidation CreateEmailValidationToken()
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new UserEmailTokenValidation
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

