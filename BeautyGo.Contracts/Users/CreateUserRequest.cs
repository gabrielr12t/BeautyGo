namespace BeautyGo.Contracts.Users;

public record CreateUserRequest(string FirstName,
    string LastName,
    string Email,
    string Password,
    string CPF,
    string Phone);
