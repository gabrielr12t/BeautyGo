namespace BeautyGo.Application.Core.Abstractions.Security;

public interface IEnvironmentVariableService
{
    string GetWindowsEnvironmentVariable(string key);

    void SetWindowsEnvironmentVariable(string key, string value);
}
