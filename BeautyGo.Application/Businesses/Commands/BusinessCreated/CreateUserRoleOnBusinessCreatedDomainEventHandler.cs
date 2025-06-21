using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

internal class CreateUserRoleOnBusinessCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedDomainEvent<Business>>
{
    private readonly IEFBaseRepository<UserRole> _userRoleRepository;
    private readonly IEFBaseRepository<User> _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CreateUserRoleOnBusinessCreatedDomainEventHandler(
        IEFBaseRepository<UserRole> userRoleRepository,
        IEFBaseRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public   Task Handle(EntityInsertedDomainEvent<Business> notification, CancellationToken cancellationToken)
    {
        //var user = await _userRepository.GetFirstOrDefaultAsync(
        //    new EntityByIdSpecification<User>(notification.Entity.OwnerId).And(
        //        new UserWithRolesSpecification()), true, cancellationToken);

        //if (user == null)
        //    throw new DomainException(DomainErrors.User.UserNotFound);

        //var ownerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.OWNER);
        //var professionalRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.PROFESSIONAL);

        //var ownerRole = await _userRoleRepository.GetFirstOrDefaultAsync(ownerRoleSpecification, cancellationToken: cancellationToken);
        //var professionalRole = await _userRoleRepository.GetFirstOrDefaultAsync(professionalRoleSpecification, cancellationToken: cancellationToken);

        //if (!user.HasRole(ownerRole))
        //    user.AddUserRole(ownerRole);

        //if (!user.HasRole(professionalRole))
        //    user.AddUserRole(professionalRole);

        //_userRepository.Update(user);

        //await _unitOfWork.SaveChangesAsync();

        return Task.CompletedTask;
    }
}
