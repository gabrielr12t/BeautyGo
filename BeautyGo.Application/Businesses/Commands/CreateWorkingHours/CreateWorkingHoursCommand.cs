using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Businesses.Commands.CreateWorkingHours;

public record CreateWorkingHoursCommand(
    Guid BusinessId,
    ICollection<WorkingHoursDto> WorkingHours) : ICommand<Result>;

public record WorkingHoursDto(DayOfWeek DayOfWeek, TimeSpan OpeningTime, TimeSpan ClosingTime);
