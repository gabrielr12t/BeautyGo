using BeautyGo.Domain.Core.Primitives;

namespace BeautyGo.Domain.Core.Errors;

public static class DomainErrors
{
    public static class General
    {
        public static Error UnProcessableRequest => new Error(
            "General.UnProcessableRequest",
            "The server could not process the request.");

        public static Error ServerError => new Error("General.ServerError", "Ocorreu um erro no servidor, contate o suporte técnico para mais detalhes.");

        public static Error ServiceNotRegistered => new Error("General.ServiceNotRegistered", "O serviço não foi registrado");

        public static Error ConstructorNotFound => new Error("General.ConstructorNotFound", "Construtor não encontrado");

        public static Error UnauthorizedUser => new Error("General.UnauthorizedUser", "Usuário não autorizado.");

        public static Error ForbidenUser => new Error("General.ForbidenUser", "Usuário não permitido executar a ação.");

        public static Error NotFound => new Error("General.NotFound", "Não encontrado.");
    }

    public static class Password
    {
        public static Error TooShort => new Error("Password.TooShort", "A senha é muito curta.");

        public static Error MissingUppercaseLetter => new Error(
            "Password.MissingUppercaseLetter",
            "A senha requer pelo menos uma letra maiúscula.");

        public static Error MissingLowercaseLetter => new Error(
            "Password.MissingLowercaseLetter",
            "A senha requer pelo menos uma letra minúscula.");

        public static Error MissingDigit => new Error(
            "Password.MissingDigit",
            "A senha requer pelo menos um dígito.");

        public static Error MissingNonAlphaNumeric => new Error(
            "Password.MissingNonAlphaNumeric",
            "A senha requer pelo menos uma letra não alfanumérica.");
    }

    public static class CPF
    {
        public static Error InvalidCpf => new Error("CPF.Invalid", "CPF inválido.");
    }

    public static class EmailValidationToken
    {
        public static Error TokenNotFound => new(
               "EmailValidationToken.TokenNotFound",
               "Registro não encontrado.");
    }

    public static class UserEmailValidationToken
    {
        public static Error TokenNotFound => new(
           "UserEmailValidationToken.TokenNotFound",
           "Registro não encontrado.");

        public static Error ExpiredToken => new(
           "UserEmailValidationToken.ExpiredToken",
           "Token expirado.");

        public static Error RequiredValidToken => new Error(
            "UserEmailValidationToken.RequiredValidToken",
            "Para acessar ao sistema é necessário confirmar o e-mail.");

        public static Error NewToken => new Error(
            "UserEmailValidationToken.NewToken",
            "Foi enviada uma nova chave para confirmação no seu e-mail.");
    }

    public static class StoreEmailValidationToken
    {
        public static Error CnpjNotFound => new(
           "UserEmailValidationToken.CnpjNotFound",
           "Cnpj não encontrado.");
    }

    public static class User
    {
        public static Error UserNotFound => new(
            "User.UserNotFound",
            "Usuário não encontrado.");

        public static Error UserNotActive => new(
            "User.UserNotActive",
            "Usuário inativo.");

        public static Error EmailNotConfirmed => new Error(
            "User.EmailNotConfirmed",
            "Para acessar ao sistema é necessário confirmar o e-mail.");

        public static Error InvalidEmail => new(
           "User.InvalidEmail",
           "E-mail inválido.");

        public static Error InvalidCPF => new(
          "User.InvalidEmail",
          "CPF inválido.");

        public static Error CPFAlreadyExists => new(
            "User.CPFAlreadyExists",
            "CPF já cadastrado.");

        public static Error EmailAlreadyExists => new(
            "User.EmailAlreadyExists",
            "E-mail já cadastrado.");
    }

    public static class Authentication
    {
        public static Error InvalidEmailOrPassword => new(
            "Authentication.InvalidEmailOrPassword",
            "E-mail ou senha incorretos.");

        public static Error InvalidRefreshToken => new(
           "Authentication.InvalidRefreshToken",
           "Autenticação inválida.");
    }

    public static class Store
    {
        public static Error InvalidCnpj => new(
            "Store.InvalidCnpj",
            "Cnpj inválido.");

        public static Error CnpjRestricted => new(
             "Store.CnpjRestricted",
             "Cnpj está com situação inválida na receira federal.");

        public static Error CnpjAlreadyExists => new(
            "Store.CnpjAlreadyExists",
            "Cnpj já cadastrado.");

        public static Error CnpjNameInvalid => new(
            "Store.CnpjNameInvalid",
            "Nome inválido para o Cnpj.");

        public static Error NoImageUploaded => new(
            "Store.NoImageUploaded",
            "Nenhuma imagem encontrada.");

        public static Error ImageExceedsMaximumAllowed => new(
            "Store.ImageExceedsMaximumAllowed",
            "Arquivo de imagem execeu o tamanha máximo permitido.");

        public static Error ImageNotValid => new(
            "Store.ImageNotValid",
            "Arquivo de imagem inválido.");
    }

    public static class Address
    {
        public static Error CepNotFound => new(
                "Address.CepNotFound",
                "Cep não encontrado.");
    }
}
