using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

public interface IEntityConfirmationHandle :
   IVisitorAsync<BusinessEmailConfirmation>,
   IVisitorAsync<UserEmailConfirmation>
{
}
