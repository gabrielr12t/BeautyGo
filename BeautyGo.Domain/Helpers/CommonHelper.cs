using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BeautyGo.Domain.Helpers;

public partial class CommonHelper
{
    #region Fields

    private const string EMAIL_EXPRESSION = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";
    private const string CPF_EXPRESSION = @"\d{11}";
    private const string CNPJ_EXPRESSION = @"(^\d{2}.\d{3}.\d{3}/\d{4}-\d{2}$)";
    private const string CEP_EXPRESSION = @"^\d{8}$";

    private static readonly Regex _emailRegex;
    private static readonly Regex _cpfRegex;
    private static readonly Regex _cnpjRegex;
    private static readonly Regex _cepRegex;

    #endregion

    #region Ctor

    static CommonHelper()
    {
        _emailRegex = new Regex(EMAIL_EXPRESSION, RegexOptions.IgnoreCase);
        _cpfRegex = new Regex(CPF_EXPRESSION, RegexOptions.IgnoreCase);
        _cnpjRegex = new Regex(CNPJ_EXPRESSION, RegexOptions.IgnoreCase);
        _cepRegex = new Regex(CEP_EXPRESSION, RegexOptions.IgnoreCase);
    }

    #endregion

    #region Levenshtein

    public static int CalculateLevenshteinDistance(string text1, string text2)
    {
        if (string.IsNullOrEmpty(text1))
            return string.IsNullOrEmpty(text2) ? 0 : text2.Length;

        if (string.IsNullOrEmpty(text2))
            return text1.Length;

        int length1 = text1.Length;
        int length2 = text2.Length;

        var matrix = new int[length1 + 1, length2 + 1];

        for (int i = 0; i <= length1; i++)
            matrix[i, 0] = i;

        for (int j = 0; j <= length2; j++)
            matrix[0, j] = j;

        for (int i = 1; i <= length1; i++)
        {
            for (int j = 1; j <= length2; j++)
            {
                int cost = (text1[i - 1] == text2[j - 1]) ? 0 : 1;

                matrix[i, j] = Math.Min(
                    Math.Min(
                        matrix[i - 1, j] + 1,
                        matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }

        return matrix[length1, length2];
    }

    public static double CalculateProximity(string text1, string text2)
    {
        int distance = CalculateLevenshteinDistance(text1, text2);
        int maxLength = Math.Max(text1.Length, text2.Length);

        // Returns the proximity as a percentage between 0 and 1 (or 0% to 100%)
        return maxLength == 0 ? 1.0 : 1.0 - (double)distance / maxLength;
    }

    public static bool CheckProximityWithThreshold(string text1, string text2, double proximityThreshold)
    {
        double proximity = CalculateProximity(text1, text2);
        return proximity >= proximityThreshold;
    }

    #endregion

    public static string RemoveWhiteSpaces(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return value.Replace(" ", string.Empty);
    }

    public static bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string cpfBase = cpf[..9];
        int soma = cpfBase
            .Select((chr, idx) => (chr - '0') * multiplicador1[idx])
            .Sum();

        int primeiroDigito = (soma % 11) < 2 ? 0 : 11 - (soma % 11);

        soma = (cpfBase + primeiroDigito)
            .Select((chr, idx) => (chr - '0') * multiplicador2[idx])
            .Sum();

        int segundoDigito = (soma % 11) < 2 ? 0 : 11 - (soma % 11);

        return cpf.EndsWith($"{primeiroDigito}{segundoDigito}");
    }

    public static bool IsValidCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = EnsureNumericOnly(cnpj);

        if (cnpj.Length != 14)
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string cnpjBase = cnpj[..12];
        int soma = cnpjBase
            .Select((chr, idx) => (chr - '0') * multiplicador1[idx])
            .Sum();

        int primeiroDigito = (soma % 11) < 2 ? 0 : 11 - (soma % 11);

        soma = (cnpjBase + primeiroDigito)
            .Select((chr, idx) => (chr - '0') * multiplicador2[idx])
            .Sum();

        int segundoDigito = (soma % 11) < 2 ? 0 : 11 - (soma % 11);

        return cnpj.EndsWith($"{primeiroDigito}{segundoDigito}");
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        email = email.Trim();

        return _emailRegex.IsMatch(email);
    }

    public static bool IsValidIpAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out var _);
    }

    public static bool IsValidCEP(string cep)
    {
        if (string.IsNullOrEmpty(cep))
            return false;

        cep = EnsureNumericOnly(cep);

        cep = cep.Trim();

        return _cepRegex.IsMatch(cep);
    }

    public static double DistanceTo(double fromLat, double fromLon, double toLat, double toLon)
    {
        double rlat1 = Math.PI * fromLat / 180;
        double rlat2 = Math.PI * toLat / 180;
        double theta = fromLon - toLon;
        double rtheta = Math.PI * theta / 180;
        double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        return dist * 1.609344;
    }

    public static string GenerateRandomDigiteCode(int length)
    {
        var random = new Random();
        var str = string.Empty;
        for (int i = 0; i < length; i++)
            str = string.Concat(str, random.Next(10).ToString());
        return str;
    }

    public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        if (str.Length <= maxLength)
            return str;

        var pLen = postfix?.Length ?? 0;

        var result = str.Substring(0, maxLength - pLen);
        if (!string.IsNullOrEmpty(postfix))
        {
            result += postfix;
        }

        return result;
    }

    public static string EnsureNumericOnly(string str)
    {
        return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());
    }

    public static string EnsureNotNull(string str)
    {
        return str ?? string.Empty;
    }

    public static bool AreNullOrEmpty(params string[] stringsToValidate)
    {
        return stringsToValidate.Any(string.IsNullOrEmpty);
    }

    public static bool ArraysEqual<T>(T[] a1, T[] a2)
    {
        if (ReferenceEquals(a1, a2))
            return true;

        if (a1 == null || a2 == null)
            return false;

        if (a1.Length != a2.Length)
            return false;

        var comparer = EqualityComparer<T>.Default;
        return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
    }

    public static void SetProperty(object instance, string propertyName, object value)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var instanceType = instance.GetType();
        var pi = instanceType.GetProperty(propertyName);
        if (pi == null)
            throw new Exception($"No property '{propertyName}' found on the instance of type '{instanceType}'.");
        if (!pi.CanWrite)
            throw new Exception($"The property '{propertyName}' on the instance of type '{instanceType}' does not have a setter.");
        if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
            value = To(value, pi.PropertyType);
        pi.SetValue(instance, value, Array.Empty<object>());
    }

    public static object To(object value, Type destinationType)
    {
        return To(value, destinationType, CultureInfo.InvariantCulture);
    }

    public static object To(object value, Type destinationType, CultureInfo culture)
    {
        if (value == null)
            return null;

        var sourceType = value.GetType();

        var destinationConverter = TypeDescriptor.GetConverter(destinationType);
        if (destinationConverter.CanConvertFrom(value.GetType()))
            return destinationConverter.ConvertFrom(null, culture, value);

        var sourceConverter = TypeDescriptor.GetConverter(sourceType);
        if (sourceConverter.CanConvertTo(destinationType))
            return sourceConverter.ConvertTo(null, culture, value, destinationType);

        if (destinationType.IsEnum && value is int @int)
            return Enum.ToObject(destinationType, @int);

        if (!destinationType.IsInstanceOfType(value))
            return Convert.ChangeType(value, destinationType, culture);

        return value;
    }

    public static T To<T>(object value)
    {
        //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        return (T)To(value, typeof(T));
    }

    public static string ConvertEnum(string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        var result = string.Empty;
        foreach (var c in str)
            if (c.ToString() != c.ToString().ToLower())
                result += " " + c.ToString();
            else
                result += c.ToString();

        //ensure no spaces (e.g. when the first letter is upper case)
        result = result.TrimStart();
        return result;
    }

    public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
    {
        //source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
        //this assumes you are looking for the western idea of age and not using East Asian reckoning.
        var age = endDate.Year - startDate.Year;
        if (startDate > endDate.AddYears(-age))
            age--;
        return age;
    }

    public static object GetPrivateFieldValue(object target, string fieldName)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target), "The assignment target cannot be null.");

        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentException("fieldName", "The field name cannot be null or empty.");

        var t = target.GetType();
        FieldInfo fi = null;

        while (t != null)
        {
            fi = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (fi != null)
                break;

            t = t.BaseType;
        }

        if (fi == null)
        {
            throw new Exception($"Field '{fieldName}' not found in type hierarchy.");
        }

        return fi.GetValue(target);
    }

    public static string Mask(string text, int startIndex, int length, string characterMask = "*")
    {
        if (string.IsNullOrEmpty(text?.Trim()))
            return string.Empty;

        //var numbers = CommonHelper.EnsureNumericOnly(text);

        //var stringBuilder = new StringBuilder(numbers);
        var stringBuilder = new StringBuilder(text);
        stringBuilder.Remove(startIndex, length);
        stringBuilder.Insert(startIndex, characterMask, length);
        return stringBuilder.ToString();
    }
}
