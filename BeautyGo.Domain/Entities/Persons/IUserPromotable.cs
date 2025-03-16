namespace BeautyGo.Domain.Entities.Persons;

public interface IUserPromotable
{
    void PromoteSpecificProperties(Guid? businessId = null);
}
