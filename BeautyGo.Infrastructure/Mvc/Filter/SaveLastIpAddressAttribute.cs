using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeautyGo.Infrastructure.Mvc.Filter;

public class SaveLastIpAddressAttribute : TypeFilterAttribute
{
    #region Ctor

    public SaveLastIpAddressAttribute() : base(typeof(SaveLastIpAddressFilter)) { }

    #endregion

    #region Nested Filter

    private class SaveLastIpAddressFilter : IAsyncActionFilter
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHelper _webHelper;
        private readonly IAuthService _authService;
        private readonly IBaseRepository<User> _userRepository;

        #endregion

        #region Ctor

        public SaveLastIpAddressFilter(IAuthService authService, IBaseRepository<User> userRepository, IUnitOfWork unitOfWork, IWebHelper webHelper)
        {
            _authService = authService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _webHelper = webHelper;
        }

        #endregion

        #region Utilities

        public async Task SaveLastIpAddressAsync(ActionExecutingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.HttpContext.Request == null)
                return;

            var cancellationToken = context.HttpContext.RequestAborted;

            var user = await _authService.GetCurrentUserAsync(cancellationToken);
            if (user != null)
            {
                user.ChangeIpAddress(await _webHelper.GetCurrentIpAddressAsync());

                _userRepository.Update(user);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        #endregion

        #region Methods

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await SaveLastIpAddressAsync(context);

            if (context.Result == null)
                await next();
        }

        #endregion
    }

    #endregion
}
