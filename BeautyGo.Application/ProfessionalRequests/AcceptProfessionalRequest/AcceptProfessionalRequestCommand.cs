using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.ProfessionalRequests.AcceptProfessionalRequest;

public record AcceptProfessionalRequestCommand(Guid ProfessionalRequestId) : ICommand<Result>;
