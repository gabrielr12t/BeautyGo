using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents.Users;

namespace BeautyGo.Domain.Entities.Users;

public abstract class User : BaseEntity, IAuditableEntity, IEmailValidationToken
{
    public User()
    {
        UserRoles = new HashSet<UserRoleMapping>();
        Passwords = new List<UserPassword>();
        Addresses = new List<UserAddressMapping>();
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

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public Guid? BillingAddressId { get; set; }

    public Guid? ShippingAddressId { get; set; }

    public ICollection<UserRoleMapping> UserRoles { get; set; }

    public ICollection<UserAddressMapping> Addresses { get; set; }
     
    public ICollection<UserPassword> Passwords { get; set; }

    #region Methods

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
        Passwords.OrderByDescending(p => p.CreatedOn).FirstOrDefault();

    public bool HasRole(UserRole role) =>
        UserRoles.Any(p => p.UserRole == role);

    public void AddUserRole(UserRole userRole) =>
        UserRoles.Add(new UserRoleMapping { UserRoleId = userRole.Id, User = this });

    public void AddUserPassword(string password, string salt) =>
        Passwords.Add(new UserPassword { User = this, Password = password, Salt = salt });

    //public abstract User Create(string firstName, string lastName, string email, string cpf);

    //public static User CreateCustomer(string firstName, string lastName, string email, string cpf)
    //{
    //    var user = new User
    //    {
    //        FirstName = firstName,
    //        LastName = lastName,
    //        Email = email,
    //        Cpf = cpf,
    //        IsActive = false,
    //        EmailConfirmed = false,
    //        CreatedOn = DateTime.Now,
    //    };

    //    user.AddDomainEvent(new EntityEmailValidationTokenCreatedEvent(user));

    //    return user;
    //}

    public BeautyGoEmailTokenValidation CreateEmailValidationToken()
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

