namespace BeautyGo.Contracts.Address;

public class AddressNearbyDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string StateAbbreviation { get; set; }
    public string Neighborhood { get; set; }
    public string Number { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedOn { get; set; }
    public double DistanceKm { get; set; }
}
