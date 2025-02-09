namespace BeautyGo.Contracts.Users.Integrations
{
    public record UserIntegrationModel(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        ICollection<string> Roles);
}
