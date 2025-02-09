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


        #endregion

        #region Ctor

        public SaveLastActivityFilter()
        {
        }

        #endregion

        #region Utilities

        public async Task SaveLastActivityAsync(ActionExecutingContext context)
        {
            //if (context == null)
            //    throw new ArgumentNullException(nameof(context));

            //if (context.HttpContext.Request == null)
            //    return;

            //var user = await _workContext.GetCurrentUserAsync();
            //if (user != null)
            //{
            //    user.LastActivityDateUtc = DateTime.UtcNow;

            //    await _userRepository.UpdateAsync(user);
            //}
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
