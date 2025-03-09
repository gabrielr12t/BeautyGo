namespace BeautyGo.Domain.Entities.Businesses;

public class BusinessWorkingHours : BaseEntity
{
    public DayOfWeek Day { get; set; }

    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; }

    public static BusinessWorkingHours Create(DayOfWeek day, TimeSpan openingTime, TimeSpan closingTime)
    {
        return new BusinessWorkingHours { Day = day, OpeningTime = openingTime, ClosingTime = closingTime };
    }
}
