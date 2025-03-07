using System.Runtime.InteropServices;
using Moss.NET.Sdk.Core;
using Moss.NET.Sdk.FFI;
using Moss.NET.Sdk.FFI.Dto;

namespace Moss.NET.Sdk.Storage;

public class EpubNotebook : StorageItem<EpubNotebook>
{
    [DllImport(Functions.DLL, EntryPoint = "moss_api_document_new_epub")]
    private static extern ulong NewEpub(ulong newEpPtr);
    
    public EpubNotebook(string name, string file, string? parent = null)
    {
        var uuid = NewEpub(name, file, parent);
        Metadata = Metadata.Get(uuid);
    }

    public EpubNotebook(string name, Base64 data, string? parent = null)
    {
        var uuid = NewEpub(name, data, parent);
        Metadata = Metadata.Get(uuid);
    }
    
    public EpubNotebook()
    {
        
    }
    
    private static string NewEpub(string name, string file, string? parent = null)
    {
        var notebook = new DocumentNewEpub
        {
            Name = name,
            Parent = parent,
            EpubFile = file,
            Accessor = new Accessor
            {
                Type = AccessorType.APIDocument
            }
        };

        return NewEpub(notebook.GetPointer()).ReadString();
    }

    private static string NewEpub(string name, Base64 data, string? parent = null)
    {
        var notebook = new DocumentNewEpub
        {
            Name = name,
            Parent = parent,
            EpubData = data,
            Accessor = new Accessor
            {
                Type = AccessorType.APIDocument
            }
        };

        return NewEpub(notebook.GetPointer()).ReadString();
    }
}