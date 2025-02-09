namespace BeautyGo.Application.Core.Abstractions.Stores;

public interface IStoreContext
{
    Guid GetCurrentStoreCode();
}
