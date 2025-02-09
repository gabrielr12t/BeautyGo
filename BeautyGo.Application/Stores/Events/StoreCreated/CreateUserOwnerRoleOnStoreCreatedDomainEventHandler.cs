using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Stores.Events.StoreCreated;

internal class CreateUserOwnerRoleOnStoreCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<Store>>
{
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IBaseRepository<User> _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CreateUserOwnerRoleOnStoreCreatedDomainEventHandler(
        IBaseRepository<UserRole> userRoleRepository,
        IBaseRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EntityInsertedEvent<Store> notification, CancellationToken cancellationToken)
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
            user.IncludeUserRole(ownerRole);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
