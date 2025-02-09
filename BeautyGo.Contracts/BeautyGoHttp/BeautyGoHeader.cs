namespace BeautyGo.Contracts.BeautyGoHttp;

public class BeautyGoHeader
{
    public BeautyGoHeader(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; set; }
    public string Value { get; set; }
}
