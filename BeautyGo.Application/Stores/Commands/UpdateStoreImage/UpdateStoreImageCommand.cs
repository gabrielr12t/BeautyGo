using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Application.Stores.Commands.UpdateStoreImage;

public record UpdateStoreImageCommand(
    Guid StoreId,
    ICollection<IFormFile> Files) : ICommand<Result>;
