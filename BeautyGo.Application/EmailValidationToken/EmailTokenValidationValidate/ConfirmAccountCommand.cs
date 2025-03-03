using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.EmailValidationToken.EmailTokenValidationValidate;

public record ConfirmAccountCommand(
    string Token) : ICommand<Result>;
