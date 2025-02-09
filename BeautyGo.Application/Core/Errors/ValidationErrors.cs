using BeautyGo.Domain.Core.Primitives;
using BeautyGo.Domain.Helpers;

namespace BeautyGo.Application.Core.Errors;

internal static class ValidationErrors
{
    /// <summary>
    /// Contains the login errors.
    /// </summary>
    internal static class Login
    {
        internal static Error EmailIsRequired => new Error("Login.EmailIsRequired", "The email is required.");

        internal static Error PasswordIsRequired => new Error("Login.PasswordIsRequired", "The password is required.");
    }

    /// <summary>
    /// Contains the change password errors.
    /// </summary>
    internal static class ChangePassword
    {
        internal static Error UserIdIsRequired => new Error("ChangePassword.UserIdIsRequired", "The user identifier is required.");

        internal static Error PasswordIsRequired => new Error("ChangePassword.PasswordIsRequired", "The password is required.");
    }

    internal static class CreateStore
    {
        internal static Error NameIsRequired => new Error("CreateStore.NameIsRequired", "O nome é obrigatório.");
        internal static Error NameInvalidLengthRequired => new Error("CreateStore.NameInvalidLengthRequired", "O nome da loja deve ter pelo menos 3 caracteres.");
        internal static Error HomePageTitleIsRequired => new Error("CreateStore.HomePageTitleIsRequired", "O título da página inicial não pode ser vazio.");
        internal static Error HomePageDescriptionIsRequired => new Error("CreateStore.HomePageDescriptionIsRequired", "A descrição da página inicial não pode ser vazia.");
        internal static Error DescriptionIsRequired => new Error("CreateStore.DescriptionIsRequired", "A descrição não pode ser vazia.");
        internal static Error CnpjIsRequired => new Error("CreateStore.CnpjIsRequired", "O CNPJ não pode ser vazio.");
        internal static Error InvalidCnpj => new Error("CreateStore.InvalidCnpj", "O CNPJ informado é inválido.");
        internal static Error CepIsRequired => new Error("CreateStore.CepIsRequired", "O CEP não pode ser vazio.");
        internal static Error InvalidCep => new Error("CreateStore.InvalidCep", "O CEP informado é inválido.");
        internal static Error PhoneIsRequired => new Error("CreateStore.PhoneIsRequired", "O telefone não pode ser vazio.");
        internal static Error InvalidPhoneFormat => new Error("CreateStore.InvalidPhoneFormat", "O telefone deve estar no formato (XX) XXXXX-XXXX.");
    }

    /// <summary>
    /// Contains the create user errors.
    /// </summary>
    internal static class CreateUser
    {
        internal static Error FirstNameIsRequired => new Error("CreateUser.FirstNameIsRequired", "O primeiro nome é obrigatório.");

        internal static Error LastNameIsRequired => new Error("CreateUser.LastNameIsRequired", "O sobrenome é obrigatório.");

        internal static Error EmailIsRequired => new Error("CreateUser.EmailIsRequired", "The email is required.");

        internal static Error EmailIsInvalid => new Error("CreateUser.EmailIsInvalid", "O email é inválido.");

        internal static Error EmailAlreadyExists => new Error("CreateUser.EmailAlreadyExists", "Email já cadastrado.");

        internal static Error PasswordIsRequired => new Error("CreateUser.PasswordIsRequired", "The password is required.");

        internal static Error SystemIsRequired => new Error("CreateUser.SystemIsRequired", "O sistema é obrigatório.");

        internal static Error CPFInvalid => new Error("CreateUser.CPFInvalid", "CPF inválido.");

        internal static Error CPFAlreadyExists => new Error("CreateUser.CPFAlreadyExists", "CPF já cadastrado.");

        internal static Error PhoneAlreadyExists => new Error("CreateUser.PhoneAlreadyExists", "Telefone já cadastrado.");
    }

    /// <summary>
    /// Contains the update user errors.
    /// </summary>
    internal static class UpdateUser
    {
        internal static Error UserIdIsRequired => new Error("UpdateUser.UserIdIsRequired", "The user identifier is required.");

        internal static Error FirstNameIsRequired => new Error("UpdateUser.FirstNameIsRequired", "The first name is required.");

        internal static Error LastNameIsRequired => new Error("UpdateUser.LastNameIsRequired", "The last name is required.");
    }
}
