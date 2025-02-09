using BeautyGo.Application.Core.Abstractions.Stores;
using BeautyGo.Domain.Common.Defaults;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace BeautyGo.Infrasctructure.Services.Stores
{
    internal class StoreContext : IStoreContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
         
        public Guid GetCurrentStoreCode()
        {
            var storeCode = _httpContextAccessor.HttpContext?.Session.Get(BeautyGoSessionDefaults.StoreCode);

            if (storeCode == null)
                throw new ArgumentException("Store code not found.");

            var storeIdString = Encoding.UTF8.GetString(storeCode);

            return Guid.Parse(storeIdString);
        }
    }
}
