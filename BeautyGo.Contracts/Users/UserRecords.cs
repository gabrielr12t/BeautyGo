namespace BeautyGo.Contracts.Users;

public record UserOnlineResponse(Guid Id, string Name, DateTime LastActivity);

public record UserResult(Guid Id, string FirstName, string LastName, ICollection<string> Roles);

public record UserIdResult(Guid Id);

public record CreateUserRequest(string FirstName, string LastName, string Email, string Password, string CPF, string Phone);

public record CreateUserResponse(Guid Id);