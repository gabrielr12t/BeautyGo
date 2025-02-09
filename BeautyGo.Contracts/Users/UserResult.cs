namespace BeautyGo.Contracts.Users
{
    public record UserResult(Guid Id, string FirstName, string LastName, ICollection<string> Roles);
}
