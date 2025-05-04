namespace BeautyGo.Contracts.Businesses;

public class BusinessFilterModel
{
    public Guid BusinessId { get; set; }
    public string BusinessName { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string State { get; set; }
    public double? DistanceKm { get; set; }
    public string StateAbbreviation { get; set; }
    public string PostalCode { get; set; }
}
