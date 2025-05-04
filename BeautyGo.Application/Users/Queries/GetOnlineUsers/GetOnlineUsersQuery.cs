using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Common.Filters;
using BeautyGo.Contracts.Users;
using BeautyGo.Domain.Core.Abstractions;

namespace BeautyGo.Application.Users.Queries.GetOnlineUsers;

public sealed record GetOnlineUsersQuery(int PageIndex, int PageSize) : FilterBase(PageIndex, PageSize), IQuery<IPagedList<UserOnlineResponse>>;
