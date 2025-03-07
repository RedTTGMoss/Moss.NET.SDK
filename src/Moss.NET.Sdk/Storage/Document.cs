using System;
using System.Collections.Generic;
using Moss.NET.Sdk.Core;

namespace Moss.NET.Sdk.Storage;

public partial class Document : StorageItem
{
    public Document(string name, string? parent = null)
    {
        ID = StorageFunctions.NewMetadata(name, RMDocumentType.DocumentType, parent);
        var uuid = NewNotebook(name, parent);
        Metadata = Metadata.Get(uuid);
    }

    public StandaloneId ID { get; set; }

    private Document()
    {

    }

    public static Document Get(string uuid)
    {
        var metadata = Metadata.Get(uuid);

        if (metadata is null)
        {
            throw new KeyNotFoundException($"No Metadata with uuid {uuid} found");
        }

        return new Document { Metadata = metadata };
    }

    /// <summary>
    /// This function randomizes a lot of the UUIDs of the document,
    /// it will also update timestamps and set the document to provision.
    /// </summary>
    /// <returns>Returns a new <see cref="Document"/> with the UUID of the new duplicate.</returns>
    public Document Duplicate()
    {
        var newId = DuplicateDocument(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer())
            .ReadString();

        return Get(newId);
    }

    /// <summary>
    /// It will halt until the document is finished downloading.
    /// </summary>
    public void EnsureDownload()
    {
        EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }

    /// <summary>
    /// Calls the callback when the document is finished downloading.
    /// </summary>
    /// <param name="callback"></param>
    public void EnsureDownload(Action callback)
    {
        EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer(), "dispatch_entry".GetPointer());

        //Todo: when EnsureDownload returns a taskid add it to the dispatcher
        //Dispatcher.Register(taskid, callback);
    }

    /// <summary>
    /// Simply unloads any files that Moss has loaded on the document.
    /// Documents will usually be automatically unloaded upon closing.
    /// If your extension loaded files though, this is the way to unload them and you should!
    /// </summary>
    public void UnloadFiles()
    {
        UnloadFiles(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }

    /// <summary>
    /// This will modify all the document UUIDs including nested UUID references to a new random UUID.
    /// </summary>
    public void RandomizeUUIDs()
    {
        var newId = RandomizeUuids(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer()).ReadString();
        Metadata = Metadata.Get(newId);
    }

    /// <summary>
    /// This function loads all the files enforcing cache usage only.
    /// If the cache misses a file, it will not be downloaded.
    /// </summary>
    public void LoadFilesFromCache()
    {
        LoadFilesFromCache(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid).GetPointer());
    }

    public void Delete(Action callback, bool unload = false)
    {
        var taskid = StorageFunctions.Delete(new Accessor {
            Type = AccessorType.APIDocument,
            Uuid = Metadata.Accessor.Uuid
        }, "dispatch_entry", unload);

        Dispatcher.Register(taskid, callback);
    }

    public void Upload(Action callback, bool unload = false)
    {
        var taskid = StorageFunctions.Upload(new Accessor {
            Type = AccessorType.APIDocument,
            Uuid = Metadata.Accessor.Uuid
        }, "dispatch_entry", unload);

        Dispatcher.Register(taskid, callback);
    }
}