using System;
using System.Collections.Generic;
using Extism;

namespace Totletheyn.Core.Lib;

public class HttpRequest(string url)
{
    public Uri url { get; set; } = new(url);

    public Dictionary<string, string> headers { get; } = new();

    public string method { get; set; }

    public byte[] body { get; set; } = [];

    public HttpResponse send()
    {
        var request = new Extism.HttpRequest(url)
        {
            Body = body, Method = Enum.Parse<HttpMethod>(method)
        };
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        var response = Pdk.SendRequest(request);

        return new HttpResponse(response.Body, response.Status);
    }
}