using BeautyGo.Application.Core.Abstractions.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeautyGo.Infrastructure.Mvc.Filter;

public sealed class BeautyGoAuthorize : TypeFilterAttribute
{
    #region Ctor

    public BeautyGoAuthorize(params string[] permissions) : base(typeof(BeautyGoAuthorizeFilter))
    {
        Arguments = [permissions];
        Permissions.AddRange(permissions);
    }

    #endregion

    #region Properties

    public List<string> Permissions { get; } = [];

    #endregion

    #region Nested filter

    private class BeautyGoAuthorizeFilter : IAsyncAuthorizationFilter
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly string[] _permissions;

        #endregion

        #region Ctor

        public BeautyGoAuthorizeFilter(
            IUserService userService,
            string[] permissions)
        {
            _userService = userService;
            _permissions = permissions;
        }

        #endregion

        #region Utilities

        public async Task AuthorizeAsync(AuthorizationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            if (context.Filters.Any(filter => filter is BeautyGoAuthorizeFilter))
            {
                foreach (var permissionRole in _permissions)
                {
                    if (await _userService.AuthorizeAsync(permissionRole, context.HttpContext.RequestAborted))
                        return;

                    context.Result = new ChallengeResult();
                }
            }
        }

        #endregion

        #region Methods

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await AuthorizeAsync(context);
        }

        #endregion
    }

    #endregion
}
