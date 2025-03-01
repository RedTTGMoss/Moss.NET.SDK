using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.API;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk.NEW;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
public static class Storage
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_metadata_get_all")]
    private static extern ulong GetDocumentMetadata(ulong uuidPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_duplicate")]
    private static extern ulong DuplicateDocument(ulong uuidPtr); // -> string

    public static Metadata GetDocumentMetadata(string uuid)
    {
        var uuidPtr = Pdk.Allocate(uuid).Offset;
        var resultPtr = GetDocumentMetadata(uuidPtr);

        return Utils.Deserialize(resultPtr, JsonContext.Default.Metadata);
    }

    public static string DuplicateDocument(string uuid)
    {
        var uuidPtr = Pdk.Allocate(uuid).Offset;
        var resultPtr = DuplicateDocument(uuidPtr);

        return MemoryBlock.Find(resultPtr).ReadString();
    }
}