namespace BeautyGo.Contracts.Users;

public record UserIntegrationModel(Guid Id, string FirstName, string LastName, string Email, ICollection<string> Roles);

public record CreateCustomerIntegrationModel(string FirstName, string Email, string Password, ICollection<string> Roles);

public record UpdateCustomerIntegrationModel(Guid Id, string FirstName, string LastName);