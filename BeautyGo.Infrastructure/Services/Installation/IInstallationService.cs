namespace BeautyGo.Infrastructure.Services.Installation
{
    public interface IInstallationService
    {
        Task<bool> IsInstalledAsync();

        Task InstallAsync();
    }
}
