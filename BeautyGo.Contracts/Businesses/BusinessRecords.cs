namespace BeautyGo.Contracts.Businesses;

public record BusinessFilter(string Name, string Description, double? Latitude = null, double? Longitude = null, double? RadiusKm = null, int PageSize = 10, int PageNumber = 1);


