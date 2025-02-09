using BeautyGo.Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace BeautyGo.Infrastructure.Mvc.Middleware;

public class StandardizedResultMiddleware
{
    private readonly RequestDelegate _next;

    public StandardizedResultMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        try
        {
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.ContentType = "application/json";

                var responseText = await FormatResponse(context.Response);
                var byteArray = Encoding.UTF8.GetBytes(responseText);
                await originalBodyStream.WriteAsync(byteArray, 0, byteArray.Length);
            }
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        using (var stream = new StreamReader(response.Body))
        {
            var responseBody = await stream.ReadToEndAsync();

            var responseBodyObject = JsonConvert.DeserializeObject<object>(responseBody);

            var result = new ApiResponse(responseBodyObject);

            return JsonConvert.SerializeObject(result);
        }
    }
}
