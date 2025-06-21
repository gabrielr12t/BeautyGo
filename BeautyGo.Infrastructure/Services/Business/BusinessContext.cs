using BeautyGo.Application.Core.Abstractions.Business;
using BeautyGo.Domain.Common.Defaults;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace BeautyGo.Infrastructure.Services.Business;

internal class BusinessContext : IBusinessContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BusinessContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetBusinessCode()
    {
        var businessCode = _httpContextAccessor.HttpContext?.Session.Get(BeautyGoSessionDefaults.BusinessCode);

        if (businessCode == null)
            throw new ArgumentException($"Item code not found: '{BeautyGoSessionDefaults.BusinessCode}'");

        var businessIdString = Encoding.UTF8.GetString(businessCode);

        return Guid.Parse(businessIdString);
    }
}
