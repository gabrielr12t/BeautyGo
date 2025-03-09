using BeautyGo.Domain.Entities.Common;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Addresses;

public class NearbyAddressessSpecification : Specification<Address>
{
    private readonly double _latitude;
    private readonly double _longitude;
    private readonly double _earthRadiusKm = 6371; //5KM por padrão

    public NearbyAddressessSpecification(double latitude, double longitude, double earthRadiusKm)
    {
        _latitude = latitude;
        _longitude = longitude;
        _earthRadiusKm = earthRadiusKm;

        ApplyOrderBy(a => _earthRadiusKm * Math.Acos(
            Math.Cos(_latitude) * Math.Cos(DegreeToRadian(a.Latitude)) *
            Math.Cos(DegreeToRadian(a.Longitude) - _longitude) +
            Math.Sin(_latitude) * Math.Sin(DegreeToRadian(a.Latitude))));
    }

    #region Utilities

    public double DegreeToRadian(double degree) =>
        degree * Math.PI / 100;

    #endregion

    public override Expression<Func<Address, bool>> ToExpression() =>
        a => (_earthRadiusKm * Math.Acos(
                Math.Cos(_latitude) * Math.Cos(DegreeToRadian(a.Latitude)) *
                Math.Cos(DegreeToRadian(a.Longitude) - _longitude) +
                Math.Sin(_latitude) * Math.Sin(DegreeToRadian(a.Latitude))
            )) <= _earthRadiusKm;
}
