namespace Moss.NET.Sdk.LayoutEngine;

public class PathResolver
{
    public string Base { get; set; }

    public virtual string Resolve(string file)
    {
        return Base + file;
    }

    public byte[] ReadBytes(string file)
    {
        return File.ReadAllBytes(Resolve(file));
    }

    public string ReadText(string file)
    {
        return File.ReadAllText(Resolve(file));
    }
}