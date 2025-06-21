using BeautyGo.Domain.Helpers;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace BeautyGo.Domain.Entities.Common;

public partial class Address : BaseEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string StateAbbreviation { get; set; }

    public string Neighborhood { get; set; }

    public string Number { get; set; }

    public string Street { get; set; }

    public string PostalCode { get; set; }

    public string PhoneNumber { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public Point? Location { get; set; }

    public void ChangeCoordinates(double latitude, double longitude)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        Latitude = latitude;
        Longitude = longitude;
        Location = geometryFactory.CreatePoint(new Coordinate(Longitude, Latitude));
    }

    public static Address Create(string firstName, string lastName, string city, string state, string stateAbbreviation,
        string neighborhood, string number, string street, string postalCode, string phoneNumber)
    {
        return new Address
        {
            FirstName = firstName,
            LastName = lastName,
            City = city,
            State = state,
            StateAbbreviation = stateAbbreviation,
            Neighborhood = neighborhood,
            Number = number,
            Street = street,
            PostalCode = CommonHelper.EnsureNumericOnly(postalCode),
            PhoneNumber = phoneNumber
        };
    }
}
