using BeautyGo.Application.Core.Abstractions.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace BeautyGo.Infrastructure.Mvc.Filter;

public class HttpsRequirementAttribute : TypeFilterAttribute
{
    #region Ctor

    public HttpsRequirementAttribute(bool ignore = false) : base(typeof(HttpRequirementFilter))
    {
        IgnoreFilter = ignore;
        Arguments = [ignore];
    }

    #endregion

    #region Properties

    public bool IgnoreFilter { get; }

    #endregion

    #region Nested Filter

    private class HttpRequirementFilter : IAsyncAuthorizationFilter
    {
        #region Fields

        protected readonly bool _ignoreFilter;
        protected readonly IWebHelper _webHelper;
        protected IWebHostEnvironment _webHostEnvironment;

        #endregion

        #region Ctor

        public HttpRequirementFilter(bool ignoreFilter, IWebHelper webHelper, IWebHostEnvironment webHostEnvironment)
        {
            _ignoreFilter = ignoreFilter;
            _webHelper = webHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region Utilities

        private Task CheckHttpsRequirementAsync(AuthorizationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var actionFilter = context.ActionDescriptor.FilterDescriptors
                .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                .Select(filterDescriptor => filterDescriptor.Filter)
                .OfType<HttpsRequirementAttribute>()
                .FirstOrDefault();

            if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                return Task.CompletedTask;

            var currentConnectionSecured = _webHelper.IsCurrentConnectionSecured();

            if (!currentConnectionSecured)
            {
                var result = new { success = false, error = "HTTPSRequired", message = "HTTPS is required" };

                context.Result = new ForbidResult();
            }

            return Task.CompletedTask;
        }

        #endregion

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await CheckHttpsRequirementAsync(context);
        }
    }

    #endregion
}
