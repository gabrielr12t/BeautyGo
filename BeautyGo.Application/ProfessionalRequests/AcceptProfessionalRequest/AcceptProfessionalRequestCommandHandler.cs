using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.Appointments;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.ProfessionalRequests.AcceptProfessionalRequest;

internal class AcceptProfessionalRequestCommandHandler : ICommandHandler<AcceptProfessionalRequestCommand, Result>
{
    #region Fields

    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<ProfessionalRequest> _professionalRequestRepository;
    private readonly IBaseRepository<Appointment> _appointmentRepository;
    private readonly IBaseRepository<Business> _businessRepository;

    #endregion

    #region Ctor

    public AcceptProfessionalRequestCommandHandler(
        IUnitOfWork unitOfWork,
        IBaseRepository<ProfessionalRequest> professionalRequestRepository,
        IAuthService authService,
        IBaseRepository<Appointment> appointmentRepository,
        IBaseRepository<Business> businessRepository)
    {
        _unitOfWork = unitOfWork;
        _professionalRequestRepository = professionalRequestRepository;
        _authService = authService;
        _appointmentRepository = appointmentRepository;
        _businessRepository = businessRepository;
    }

    #endregion

    #region Utilities

    private Result ValidateProfessionalRequest(ProfessionalRequest professionalRequest, Guid currentUserId)
    {
        if (professionalRequest == null)
            return Result.Failure(DomainErrors.ProfessionalRequest.NotFound);

        if (professionalRequest.UserId != currentUserId)
            return Result.Failure(DomainErrors.General.ForbidenUser);

        if (professionalRequest.IsAccepted())
            return Result.Failure(DomainErrors.ProfessionalRequest.AlreadyAccepted);

        if (professionalRequest.IsExpired())
            return Result.Failure(DomainErrors.ProfessionalRequest.Expired(professionalRequest.ExpireAt));

        return Result.Success();
    }

    private async Task<bool> HasConfirmedAppointmentsAsync(User professional, CancellationToken cancellationToken)
    {
        var appointmentByProfessionalIdSpec = new AppointmentByProfessionalId(professional.Id);
        var confirmedAppointmentSpec = new ConfirmedAppointmentSpecification();

        var appointmentSpecification = appointmentByProfessionalIdSpec.And(confirmedAppointmentSpec);

        return await _appointmentRepository.ExistAsync(appointmentSpecification, cancellationToken: cancellationToken);
    }

    #endregion

    public async Task<Result> Handle(AcceptProfessionalRequestCommand request, CancellationToken cancellationToken)
    {
        var professionalRequest = await _professionalRequestRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<ProfessionalRequest>(request.ProfessionalRequestId), true, cancellationToken: cancellationToken);

        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        var professionalRequestValidateResult = ValidateProfessionalRequest(professionalRequest, currentUser.Id);
        if (professionalRequestValidateResult.IsFailure)
            return professionalRequestValidateResult;

        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Business>(professionalRequest.BusinessId), cancellationToken: cancellationToken);

        if (business is null)
            return Result.Failure(DomainErrors.Business.BusinessNotFound(business.Id));

        if (!business.CanWork())
            return Result.Failure(DomainErrors.Business.CannotWork);

        if (await HasConfirmedAppointmentsAsync(currentUser, cancellationToken))
            return Result.Failure(DomainErrors.Professional.ProfessionalHasConfirmedAppointments);

        if (currentUser is not Professional)
            currentUser.PromoteToProfessional(business.Id);

        professionalRequest.Accept();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
