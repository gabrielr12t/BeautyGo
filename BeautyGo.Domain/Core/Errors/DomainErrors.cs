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

    public static class Event
    {
        public static Error MaxAttemps => new Error("Event.MaxAttemps", "Tentativa máxima de executar o evento");
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

    public static class BusinessEmailValidationToken
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

        public static Error InvalidPhoneNumber => new(
           "User.InvalidPhoneNumber",
           "Número de telefone inválido.");

        public static Error InvalidCPF => new(
          "User.InvalidEmail",
          "CPF inválido.");

        public static Error CPFAlreadyExists => new(
            "User.CPFAlreadyExists",
            "CPF já cadastrado.");

        public static Error EmailAlreadyExists => new(
            "User.EmailAlreadyExists",
            "E-mail já cadastrado.");

        public static Error PhoneNumberAlreadyExists => new(
            "User.PhoneNumberAlreadyExists",
            "Telefone já cadastrado.");
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

    public static class Business
    {
        public static Error InvalidCnpj(string cnpj) => new(
            "Business.InvalidCnpj",
            $"Cnpj '{cnpj}' inválido.");

        public static Error CnpjRestricted(string cnpj) => new(
             "Business.CnpjRestricted",
             $"Cnpj '{cnpj}' está com situação inválida na receira federal.");

        public static Error CnpjAlreadyExists => new(
            "Business.CnpjAlreadyExists",
            "Cnpj já cadastrado.");

        public static Error CnpjNameInvalid(string cnpj) => new(
            "Business.CnpjNameInvalid",
            $"Nome inválido para o Cnpj '{cnpj}' informado.");

        public static Error NoImageUploaded => new(
            "Business.NoImageUploaded",
            "Nenhuma imagem encontrada.");

        public static Error ImageExceedsMaximumAllowed => new(
            "Business.ImageExceedsMaximumAllowed",
            "Arquivo de imagem execeu o tamanha máximo permitido.");

        public static Error ImageNotValid => new(
            "Business.ImageNotValid",
            "Arquivo de imagem inválido.");

        public static Error BusinessNotFound(Guid businessId) => new(
            "Business.BusinessNotFound",
            $"Loja '{businessId}' não encontrada.");

        public static Error BusinessNotFoundToUser(Guid businessId, string user) => new(
            "Business.BusinessNotFoundToUser",
            $"Loja '{businessId}' não encontrada para o usuário '{user}'.");
    }

    public static class Address
    {
        public static Error CepNotFound => new(
                "Address.CepNotFound",
                "Cep não encontrado.");

        public static Error CoordinatesNotFoundToBusiness(Guid businessId) => new(
                "Address.CoordinatesNotFoundToBusiness",
                $"Coordenadas não encontrada para a loja: '{businessId}'.");
    }
}
