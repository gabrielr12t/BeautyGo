namespace BeautyGo.Domain.Entities.Users;

public class UserPassword : BaseEntity
{
    public string Password { get; set; }
    public string Salt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public static UserPassword Create(string password, string salt)
    {
        return new UserPassword { Password = password, Salt = salt };
    }
}
