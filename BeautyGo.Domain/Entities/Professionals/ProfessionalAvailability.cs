﻿namespace BeautyGo.Domain.Entities.Professionals;

public class ProfessionalAvailability : BaseEntity
{
    public Guid ProfessionalId { get; set; }
    public Professional Professional { get; set; }

    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public TimeSpan StartLunchTime { get; set; }
    public TimeSpan EndLunchTime { get; set; }
}
