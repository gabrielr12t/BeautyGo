using Newtonsoft.Json;

namespace BeautyGo.Contracts.BeautyGoHttp;

public class BeautyGoHttpResponse<T> where T : class
{
    public BeautyGoHttpResponse()
    {

    }

    public BeautyGoHttpResponse(T data, Guid requestId, string error = null)
    {
        Data = data;
        Error = error;
    }

    public Guid RequestId { get; set; }

    public T Data { get; set; }

    public string Error { get; set; }

    public int StatusCode { get; set; }

    public bool HasError { get { return !string.IsNullOrEmpty(Error); } }

    public override string ToString() => JsonConvert.SerializeObject(this);
}
