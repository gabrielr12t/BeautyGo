using BeautyGo.Domain.Entities.Business;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

public interface IEntityValidationTokenHandle :
   IVisitorAsync<BeautyBusinessEmailTokenValidation>,
   IVisitorAsync<UserEmailTokenValidation>
{
}
