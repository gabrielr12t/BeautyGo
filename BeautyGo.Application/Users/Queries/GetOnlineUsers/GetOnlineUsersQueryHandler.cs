using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Users;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Application.Users.Queries.GetOnlineUsers;

internal class GetOnlineUsersQueryHandler : IQueryHandler<GetOnlineUsersQuery, IPagedList<UserOnlineResponse>>
{
    private IEFBaseRepository<User> _userRepository;

    public GetOnlineUsersQueryHandler(
        IEFBaseRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IPagedList<UserOnlineResponse>> Handle(GetOnlineUsersQuery request, CancellationToken cancellationToken)
    {
        var onlineUserSpec = new UserOnlineSpecification();
        Func<User, UserOnlineResponse> userSelect = response => new UserOnlineResponse(response.Id, response.FirstName, response.LastActivityDate);

        var onlineUsers = await _userRepository.GetAllPagedAsync(
            onlineUserSpec,
            userSelect,
            request.PageIndex,
            request.PageSize, false,
            cancellationToken);

        return onlineUsers;
    }
}
