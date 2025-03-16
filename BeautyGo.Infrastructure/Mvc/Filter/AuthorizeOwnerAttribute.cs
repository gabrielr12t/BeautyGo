using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Domain.Common.Defaults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeautyGo.Infrastructure.Mvc.Filter;

public sealed class AuthorizeOwnerAttribute : TypeFilterAttribute
{
    public AuthorizeOwnerAttribute() : base(typeof(AuthorizeOwnerFilter))
    {
    }

    #region Nested filter

    private class AuthorizeOwnerFilter : IAsyncAuthorizationFilter
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Ctor

        public AuthorizeOwnerFilter(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Utilities

        public async Task AuthorizeOwnerAsync(AuthorizationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            if (context.Filters.Any(filter => filter is AuthorizeOwnerFilter))
                if (!await _userService.AuthorizeAsync(BeautyGoUserRoleDefaults.OWNER, context.HttpContext.RequestAborted))
                    context.Result = new ChallengeResult();
        }

        #endregion

        #region Methods

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await AuthorizeOwnerAsync(context);
        }

        #endregion
    }

    #endregion
}
