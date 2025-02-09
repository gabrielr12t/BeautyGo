namespace BeautyGo.Domain.Entities.Users;

public class UserRoleMapping : BaseEntity
{
    public UserRole UserRole { get; set; }
    public Guid UserRoleId { get; set; }

    public User User { get; set; }
    public Guid UserId { get; set; }

    public static UserRoleMapping Create(UserRole userRole, User user) =>
        new UserRoleMapping { User = user, UserRole = userRole, UserRoleId = userRole.Id };
}
