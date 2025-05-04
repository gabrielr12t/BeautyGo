using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeautyGo.Infrastructure.Mvc.Filter;

public class SaveLastActivityAttribute : TypeFilterAttribute
{
    #region Ctor

    public SaveLastActivityAttribute() : base(typeof(SaveLastActivityFilter)) { }

    #endregion

    #region Nested Filter

    private class SaveLastActivityFilter : IAsyncActionFilter
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IBaseRepository<User> _userRepository;

        #endregion

        #region Ctor

        public SaveLastActivityFilter(IAuthService authService, IBaseRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Utilities

        public async Task SaveLastActivityAsync(ActionExecutingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.HttpContext.Request == null)
                return;

            var cancellationToken = context.HttpContext.RequestAborted;

            var user = await _authService.GetCurrentUserAsync(cancellationToken);
            if (user != null)
            {
                user.LastActivityDate = DateTime.Now;

                _userRepository.Update(user);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        #endregion

        #region Methods

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await SaveLastActivityAsync(context);

            if (context.Result == null)
                await next();
        }

        #endregion
    }

    #endregion
}
