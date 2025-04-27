using Azure.Core;
using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.Businesses;
using BeautyGo.Domain.Repositories;
using System.Threading;

namespace BeautyGo.Application.Businesses.Commands.CreateWorkingHours;

internal class CreateWorkingHoursCommandHandler : ICommandHandler<CreateWorkingHoursCommand, Result>
{
    #region Fields

    private readonly IAuthService _authService;
    private readonly IBaseRepository<Business> _businessRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public CreateWorkingHoursCommandHandler(
        IAuthService authService,
        IBaseRepository<Business> businessRepository,
        IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Utilities

    private bool HasDuplicateDayOfWeek(IEnumerable<WorkingHoursDto> workingHours)
    {
        var daysOfWeek = workingHours.Select(workingHours => workingHours.DayOfWeek).ToList();
        return daysOfWeek.Distinct().Count() != daysOfWeek.Count;
    }

    private bool OpeningIsMoreThanClosing(WorkingHoursDto workingHours) =>
        workingHours.OpeningTime >= workingHours.ClosingTime;

    private bool HasInvalidTimeRange(IEnumerable<WorkingHoursDto> workingHours) =>
        workingHours.Any(OpeningIsMoreThanClosing);

    private async Task<Business> GetBusinessAsync(Guid businessId, Guid businessOwnerId, CancellationToken cancellationToken)
    {
        var businessOwnerSpecification = new BusinessOwnerSpecification(businessOwnerId);
        var businessByIdSpecification = new EntityByIdSpecification<Business>(businessId);

        var businesSpec = businessByIdSpecification
            .And(businessOwnerSpecification)
            .AddInclude(p => p.WorkingHours);

        return await _businessRepository.GetFirstOrDefaultAsync(businesSpec, true, cancellationToken);
    }

    #endregion

    #region Handle

    public async Task<Result> Handle(CreateWorkingHoursCommand request, CancellationToken cancellationToken)
    { 
        if (HasDuplicateDayOfWeek(request.WorkingHours))
            return Result.Failure(DomainErrors.WorkingHours.DuplicateDayOfWeek);

        if (HasInvalidTimeRange(request.WorkingHours))
            return Result.Failure(DomainErrors.WorkingHours.InvalidTimeRange);

        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        var business = await GetBusinessAsync(request.BusinessId, currentUser.Id, cancellationToken);

        if (business is null)
            return Result.Failure(DomainErrors.Business.BusinessNotFoundToUser(request.BusinessId, currentUser.Email));

        if (business.HasWorkingHours())
            return Result.Failure(DomainErrors.Business.BusinessAlreadyWorkingHoursRegistered);

        var requestHasWorkingHours = request.WorkingHours is not null && request.WorkingHours.Any();
        if (requestHasWorkingHours)
        {
            var workingHours = request.WorkingHours.Select(p => BusinessWorkingHours.Create(p.DayOfWeek, p.OpeningTime, p.ClosingTime));
            business.AddWorkingHours(workingHours);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion
}
