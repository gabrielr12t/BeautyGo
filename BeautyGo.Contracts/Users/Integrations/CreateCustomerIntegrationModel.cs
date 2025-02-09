namespace BeautyGo.Contracts.Users.Integrations
{
    public record CreateCustomerIntegrationModel(
        string FirstName,
        string Email,
        string Password,
        ICollection<string> Roles);
}
