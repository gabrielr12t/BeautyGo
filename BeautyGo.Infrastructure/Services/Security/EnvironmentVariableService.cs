using BeautyGo.Application.Core.Abstractions.Security;

namespace BeautyGo.Infrasctructure.Services.Security
{
    internal class EnvironmentVariableService : IEnvironmentVariableService
    {
        public string GetWindowsEnvironmentVariable(string key)
            => Environment.GetEnvironmentVariable(key, target: EnvironmentVariableTarget.Machine);

        public void SetWindowsEnvironmentVariable(string key, string value)
            => Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Machine);
    }
}
