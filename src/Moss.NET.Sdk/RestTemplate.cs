using Extism;
using PipelineNet.Middleware;
using PipelineNet.MiddlewareResolver;
using PipelineNet.Pipelines;

public class RestTemplate
{
    private readonly Pipeline<HttpRequest> _pipeline = new(new ActivatorMiddlewareResolver());
    public Dictionary<string, string> Headers { get; } = new();

    public void Use<T>()
        where T : IMiddleware<HttpRequest, HttpResponse>
    {
        _pipeline.Add(typeof(T));
    }

    public HttpResponse Execute(HttpRequest request)
    {
        foreach (var header in Headers) request.Headers.Add(header.Key, header.Value);

        _pipeline.Execute(request);

        return Pdk.SendRequest(request);
    }

    public HttpResponse Exchange(string url)
    {
        return Execute(new HttpRequest(url));
    }

    public byte[] GetBytes(string url)
    {
        return Exchange(url).Body.ReadBytes();
    }

    public string GetString(string url)
    {
        return Exchange(url).Body.ReadString();
    }
}