namespace BeautyGo.Domain.Entities.Business;

public class BusinessWorkingHours : BaseEntity
{
    public DayOfWeek Day { get; set; }

    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; } 

    public Guid BeautyBusinessId { get; set; }
    public BeautyBusiness BeautyBusiness { get; set; }
}
