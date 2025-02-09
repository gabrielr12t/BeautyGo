namespace BeautyGo.Domain.Core.Primitives;

public sealed class Error : IEquatable<Error>
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }

    public string Message { get; }

    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

    internal static Error None => new Error(string.Empty, string.Empty);

    public static bool operator ==(Error a, Error b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Error a, Error b) => !(a == b);

    public bool Equals(Error other) => !(other is null) && GetAtomicValues().SequenceEqual(other.GetAtomicValues());

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        if (!(obj is Error valueObject))
            return false;

        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        HashCode hashCode = default;

        foreach (object obj in GetAtomicValues())
            hashCode.Add(obj);

        return hashCode.ToHashCode();
    }

    private IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
        yield return Message;
    }
}
