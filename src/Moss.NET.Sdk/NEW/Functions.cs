using System.Runtime.InteropServices;

namespace Moss.NET.Sdk.NEW;

//refers to https://redttg.gitbook.io/moss/extensions/host-functions
public static class Functions
{
    private const string DLL = "extism";

    [DllImport(DLL, EntryPoint = "moss_api_document_get")]
    public static extern ulong GetApiDocument(ulong uuidPtr, ulong keyPtr); // -> ConfigGet

    //moss_api_document_get_all(document_uuid: str) -> RM_Document
    [DllImport(DLL, EntryPoint = "moss_api_document_get_all")]
    public static extern ulong GetApiDocumentAll(ulong uuidPtr); // -> ConfigGet[RMDocument]

    //moss_em_export_statistical_data()

}