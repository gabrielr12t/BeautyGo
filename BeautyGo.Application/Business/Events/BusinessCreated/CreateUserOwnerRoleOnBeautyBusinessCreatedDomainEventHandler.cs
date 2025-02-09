using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Business;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Business.Events.BusinessCreated;

internal class CreateUserOwnerRoleOnBeautyBusinessCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<BeautyBusiness>>
{
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IBaseRepository<User> _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CreateUserOwnerRoleOnBeautyBusinessCreatedDomainEventHandler(
        IBaseRepository<UserRole> userRoleRepository,
        IBaseRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EntityInsertedEvent<BeautyBusiness> notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<User>(notification.Entity.OwnerId).And(
                new UserWithRolesSpecification()));

        if (user == null)
            throw new DomainException(DomainErrors.User.UserNotFound);

        var ownerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.OWNER);
        var ownerRole = await _userRoleRepository.GetFirstOrDefaultAsync(ownerRoleSpecification, cancellationToken: cancellationToken);

        if (!user.HasRole(ownerRole))
        {
            user.AddUserRole(ownerRole);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
