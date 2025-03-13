using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Professionals.CreateProfessional;

public record CreateProfessionalCommand(
    Guid BusinessId,
    string ProfessionalEmail,
    string ProfessionalCpf,
    string ProfessionalPhoneNumber)
    : ICommand<Result>;
