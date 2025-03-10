﻿using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

internal class CreateUserRoleOnBusinessCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<Business>>
{
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IBaseRepository<User> _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CreateUserRoleOnBusinessCreatedDomainEventHandler(
        IBaseRepository<UserRole> userRoleRepository,
        IBaseRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EntityInsertedEvent<Business> notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<User>(notification.Entity.OwnerId).And(
                new UserWithRolesSpecification()), true, cancellationToken);

        if (user == null)
            throw new DomainException(DomainErrors.User.UserNotFound);

        var ownerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.OWNER);
        var professionalRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.PROFESSIONAL);

        var ownerRole = await _userRoleRepository.GetFirstOrDefaultAsync(ownerRoleSpecification, cancellationToken: cancellationToken);
        var professionalRole = await _userRoleRepository.GetFirstOrDefaultAsync(professionalRoleSpecification, cancellationToken: cancellationToken);

        if (!user.HasRole(ownerRole))
        {
            user.AddUserRole(ownerRole);
        }

        if (!user.HasRole(professionalRole))
        {
            user.AddUserRole(professionalRole);
        }

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync();
    }
}
