using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.NEW;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
public static class Storage
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_metadata_get_all")]
    private static extern ulong GetMetadata(ulong uuidPtr);

    public static Metadata GetMetadata(string uuid)
    {
        var uuidPtr = Pdk.Allocate(uuid).Offset;
        var resultPtr = GetMetadata(uuidPtr);

        return Utils.Deserialize(resultPtr, ApiJsonContext.Default.Metadata);
    }
}