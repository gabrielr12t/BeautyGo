using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.EmailValidationToken.EmailConfirmationValidate;

public record ConfirmAccountCommand(
    string Token) : ICommand<Result>;
