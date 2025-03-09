using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Businesses.Commands.CreateBusiness;

public record CreateBusinessCommand(
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
    string AddressPhoneNumber) : ICommand<Result>;

