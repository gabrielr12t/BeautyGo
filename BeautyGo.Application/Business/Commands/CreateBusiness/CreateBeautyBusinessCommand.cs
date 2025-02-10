using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Business.Commands.CreateBusiness;

public record CreateBeautyBusinessCommand(
    string Name,
    string HomePageTitle,
    string HomePageDescription,
    string Description,
    string Cnpj,
    string Phone,
    string AddressFirstName,
    string AddressLastName,
    string AddressCep,
    string AddressNumber,
    string AddressPhoneNumber,
    ICollection<WorkingHoursDto> WorkingHours) : ICommand<Result>;

public record WorkingHoursDto(DayOfWeek Day, TimeSpan StartTime, TimeSpan EndTime);
 
