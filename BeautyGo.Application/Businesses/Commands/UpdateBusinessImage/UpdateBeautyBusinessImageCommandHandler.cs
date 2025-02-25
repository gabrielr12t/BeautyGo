using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Media;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Application.Businesses.Commands.UpdateBusinessImage;

internal class UpdateBeautyBusinessImageCommandHandler : ICommandHandler<UpdateBeautyBusinessImageCommand, Result>
{
    private readonly IBaseRepository<Domain.Entities.Businesses.Business> _businessRepository;
    private readonly IBaseRepository<User> _userRepository;

    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPictureService _pictureService;

    public UpdateBeautyBusinessImageCommandHandler(IBaseRepository<Domain.Entities.Businesses.Business> businessRepository,
        IBaseRepository<User> userRepository,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IPictureService pictureService)
    {
        _businessRepository = businessRepository;
        _userRepository = userRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
    }

    #region Utilities

    private Result BussinessValidation(UpdateBeautyBusinessImageCommand request, CancellationToken cancellationToken)
    {
        //COLOCAR ESSA CONFIG EM ALGUM SETTINGS
        long MaxFileSize = 50 * 1024 * 1024; // 50MB
        var allowedImageContentTypes = new List<string>
        {
            "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp"
        };

        if (request.Files == null || !request.Files.Any())
            return Result.Failure(DomainErrors.Business.NoImageUploaded);

        foreach (var file in request.Files)
        {
            if (file.Length > MaxFileSize)
                return Result.Failure(DomainErrors.Business.ImageExceedsMaximumAllowed);

            if (!allowedImageContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                return Result.Failure(DomainErrors.Business.ImageNotValid);
        }

        return Result.Success();
    }

    private async Task AddPicturesAsync(IEnumerable<IFormFile> files, Domain.Entities.Businesses.Business store)
    {
        foreach (var file in files)
        {
            var picture = await _pictureService.InsertPictureAsync(file);
            store.AddPicture(picture);
        }
    }

    #endregion

    public async Task<Result> Handle(UpdateBeautyBusinessImageCommand request, CancellationToken cancellationToken)
    {
        if (request.StoreId == Guid.Empty)
            return Result.Failure(DomainErrors.General.NotFound);

        var currentUser = await _authService.GetCurrentUserAsync();

        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Domain.Entities.Businesses.Business>(request.StoreId));

        if (business == null || currentUser == null)
            return Result.Failure(DomainErrors.General.NotFound);

        //PASSAR PARA UM SPECIFICATION
        var isOwnedByUser = business.CreatedId == currentUser.Id;
        if (!isOwnedByUser)
            return Result.Failure(DomainErrors.General.ForbidenUser);

        await AddPicturesAsync(request.Files, business);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
