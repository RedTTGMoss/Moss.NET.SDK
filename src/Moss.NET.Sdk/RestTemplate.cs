using Extism;
using PipelineNet.Middleware;
using PipelineNet.MiddlewareResolver;
using PipelineNet.Pipelines;

public class RestTemplate
{
    private readonly Pipeline<HttpRequest> _pipeline = new(new ActivatorMiddlewareResolver());

    public void Use<T>()
        where T : IMiddleware<HttpRequest, HttpResponse>
    {
        _pipeline.Add(typeof(T));
    }

    public HttpResponse Execute(HttpRequest request)
    {
        _pipeline.Execute(request);

        return Pdk.SendRequest(request);
    }

    public HttpResponse Exchange(string url)
    {
        return Execute(new HttpRequest(url));
    }
}