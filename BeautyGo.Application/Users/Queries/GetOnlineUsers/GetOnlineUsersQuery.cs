using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Common.Filters;
using BeautyGo.Contracts.Users;
using BeautyGo.Domain.Core.Abstractions;

namespace BeautyGo.Application.Users.Queries.GetOnlineUsers;

public sealed record GetOnlineUsersQuery(FilterBase Filter) : IQuery<IPagedList<UserOnlineResponse>>;
