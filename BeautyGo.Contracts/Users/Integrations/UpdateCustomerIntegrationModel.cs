namespace BeautyGo.Contracts.Users.Integrations
{
    public record UpdateCustomerIntegrationModel(
        Guid Id,
        string FirstName,
        string LastName);
}
