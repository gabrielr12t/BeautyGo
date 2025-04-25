using BeautyGo.Domain.Core.Primitives;

namespace BeautyGo.Application.Core.Errors;

internal static class ValidationErrors
{
    internal static class AcceptProfessionalRequest 
    {
        internal static Error ProfessionalRequestIdIsRequired => new Error(
            "AcceptProfessionalRequest.ProfessionalRequestIdIsRequired",
            "O identificador do convite é obrigatório.");
    }

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

    internal static class CreateBusiness
    {
        internal static Error NameIsRequired => new Error("CreateBusiness.NameIsRequired", "O nome é obrigatório.");
        internal static Error NameInvalidLengthRequired => new Error("CreateBusiness.NameInvalidLengthRequired", "O nome da loja deve ter pelo menos 3 caracteres.");
        internal static Error HomePageTitleIsRequired => new Error("CreateBusiness.HomePageTitleIsRequired", "O título da página inicial não pode ser vazio.");
        internal static Error HomePageDescriptionIsRequired => new Error("CreateBusiness.HomePageDescriptionIsRequired", "A descrição da página inicial não pode ser vazia.");
        internal static Error DescriptionIsRequired => new Error("CreateBusiness.DescriptionIsRequired", "A descrição não pode ser vazia.");
        internal static Error CnpjIsRequired => new Error("CreateBusiness.CnpjIsRequired", "O CNPJ não pode ser vazio.");
        internal static Error InvalidCnpj => new Error("CreateBusiness.InvalidCnpj", "O CNPJ informado é inválido.");
        internal static Error CepIsRequired => new Error("CreateBusiness.CepIsRequired", "O CEP não pode ser vazio.");
        internal static Error InvalidCep => new Error("CreateBusiness.InvalidCep", "O CEP informado é inválido.");
        internal static Error PhoneIsRequired => new Error("CreateBusiness.PhoneIsRequired", "O telefone não pode ser vazio.");
        internal static Error InvalidPhoneFormat => new Error("CreateBusiness.InvalidPhoneFormat", "O telefone deve estar no formato (XX) XXXXX-XXXX.");
    }

    internal static class CreateWorkingHours
    {
        internal static Error IsEmptyList => new Error("CreateWorkingHours.IsEmptyList", "A lista está vazia.");
        internal static Error ProvideForUpToSevenDays => new Error("CreateWorkingHours.ProvideForUpToSevenDays", "A lista está vazia.");
        internal static Error InvalidDayOfWeek => new Error("CreateWorkingHours.InvalidDayOfWeek", "Dia da semana é obrigatório.");
        internal static Error InvalidOpeningTime => new Error("CreateWorkingHours.InvalidOpeningTime", "Horário de abertura inválido.");
        internal static Error InvalidClosingTime => new Error("CreateWorkingHours.InvalidClosingTime", "Horário de fechamento inválido.");
        internal static Error ClosingTimeNotMustBeAfterOpeningTime => new Error("CreateWorkingHours.ClosingTimeNotMustBeAfterOpeningTime", "Horário de fechamento não pode ser menor que horário de abertura.");
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
