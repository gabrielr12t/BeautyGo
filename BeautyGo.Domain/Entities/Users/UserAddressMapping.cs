using BeautyGo.Domain.Entities.Common;

namespace BeautyGo.Domain.Entities.Users;

public class UserAddressMapping : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid AddressId { get; set; }
    public Address Address { get; set; }
}
