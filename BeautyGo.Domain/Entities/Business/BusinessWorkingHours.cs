namespace BeautyGo.Domain.Entities.Business;

public class BusinessWorkingHours : BaseEntity
{
    public DayOfWeek Day { get; set; }

    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; } 

    public Guid BeautyBusinessId { get; set; }
    public BeautyBusiness BeautyBusiness { get; set; }

    public static BusinessWorkingHours Create(DayOfWeek day, TimeSpan openTime, TimeSpan endTime)
    {
        var workingHour = new BusinessWorkingHours { Day = day, OpenTime = openTime, CloseTime = endTime };

        return workingHour;
    }
}
