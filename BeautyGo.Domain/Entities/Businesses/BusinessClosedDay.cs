namespace BeautyGo.Domain.Entities.Businesses;

public class BusinessClosedDay : BaseEntity
{
    public Business Business { get; set; }
    public Guid BusinessId { get; set; }

    public DateTime ClosedDate { get; set; }

    public string Reason { get; set; }
}
