namespace BeautyGo.Application.Core.Abstractions.Cryptography;

public interface IPasswordHashChecker
{
    bool HashesMatch(string passwordHash, string providedPassword);
}
