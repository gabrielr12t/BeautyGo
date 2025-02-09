using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

public interface IEntityValidationTokenHandle :
   IVisitorAsync<StoreEmailTokenValidation>,
   IVisitorAsync<UserEmailTokenValidation>
{
}
