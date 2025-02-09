using BeautyGo.Domain.Helpers;

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

    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public void ChangeCoordinates(string latitude, string longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
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
            PhoneNumber = phoneNumber,
        };
    }
}
