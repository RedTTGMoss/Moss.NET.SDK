using System;
using System.Collections.Generic;
using System.IO;
using File = Moss.NET.Sdk.FFI.File;

namespace Moss.NET.Sdk;

public static class Assets
{
    private static readonly List<File> Files = [];
    private static bool AlreadyExposed;

    public static void Add(string key, string path)
    {
        Files.Add(new File(key, path));
    }

    public static void Add(string path)
    {
        if (AlreadyExposed) throw new InvalidOperationException("Assets can only be added upon extension registration");

        Add(Path.GetFileNameWithoutExtension(path), path);
    }

    internal static List<File> Expose()
    {
        AlreadyExposed = true;
        return Files;
    }
}