namespace BeautyGo.Domain.Entities.Users;

public class UserRole : BaseEntity
{
    public string Description { get; set; }

    public ICollection<UserRoleMapping> UserRoleMappings { get; set; }
}
