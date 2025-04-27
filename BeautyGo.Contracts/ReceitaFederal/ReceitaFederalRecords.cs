namespace BeautyGo.Contracts.ReceitaFederal;

public record ReceitaFereralIntegrationResponse(string Status);

public record CnpjIntegrationResponse(string Status, string Situacao, string Nome, string Email) : ReceitaFereralIntegrationResponse(Status);
