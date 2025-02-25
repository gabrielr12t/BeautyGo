using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Application.Businesses.Commands.UpdateBusinessImage;

public record UpdateBeautyBusinessImageCommand(
    Guid StoreId,
    ICollection<IFormFile> Files) : ICommand<Result>;
