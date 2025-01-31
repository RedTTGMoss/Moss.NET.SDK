using System.Runtime.InteropServices;

namespace Moss.NET.Sdk.FFI.New;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
internal static class Functions
{
    internal const string DLL = "extism";

    [DllImport(DLL, EntryPoint = "document_metadata_get")]
    public static extern void GetDocumentMetadata(ulong keyPtr); // -> ConfigGet

    [DllImport(DLL, EntryPoint = "collection_metadata_get")]
    public static extern void GetCollectionMetadata(ulong keyPtr); // -> ConfigGet

    [DllImport(DLL, EntryPoint = "document_set_provision")]
    public static extern void SetDocumentProvision(bool toggle);

    [DllImport(DLL, EntryPoint = "document_get_provision")]
    public static extern bool GetDocumentProvision();

    [DllImport(DLL, EntryPoint = "document_metadata_set")]
    public static extern void SetDocumentMetadata(ulong configSetPTr);
}