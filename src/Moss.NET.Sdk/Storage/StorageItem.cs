using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moss.NET.Sdk.Core;

namespace Moss.NET.Sdk.Storage;

public abstract class StorageItem<TOut> where TOut : StorageItem<TOut>, new()
{
    public Metadata Metadata { get; protected set; } = null!;

    /// <summary>
    ///     It will halt until the document is finished downloading.
    /// </summary>
    public void EnsureDownload()
    {
        DocumentFunctions.EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid!).GetPointer());
    }

    /// <summary>
    ///     Calls the callback when the document is finished downloading.
    /// </summary>
    /// <param name="callback"></param>
    public void EnsureDownload(Action callback)
    {
        DocumentFunctions.EnsureDownload(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid!).GetPointer(),
            "dispatch_entry".GetPointer());

        //Todo: when EnsureDownload returns a taskid add it to the dispatcher
        //Dispatcher.Register(taskid, callback);
    }

    /// <summary>
    ///     Simply unloads any files that Moss has loaded on the document.
    ///     Documents will usually be automatically unloaded upon closing.
    ///     If your extension loaded files though, this is the way to unload them and you should!
    /// </summary>
    public void UnloadFiles()
    {
        DocumentFunctions.UnloadFiles(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid!).GetPointer());
    }

    /// <summary>
    ///     This will modify all the document UUIDs including nested UUID references to a new random UUID.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public void RandomizeUUIDs()
    {
        var newId = DocumentFunctions
            .RandomizeUuids(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid!).GetPointer()).ReadString();
        Metadata = Metadata.Get(newId);
    }

    /// <summary>
    ///     This function loads all the files enforcing cache usage only.
    ///     If the cache misses a file, it will not be downloaded.
    /// </summary>
    public void LoadFilesFromCache()
    {
        DocumentFunctions.LoadFilesFromCache(InternalFunctions.GetAccessor(Metadata.Accessor.Uuid!).GetPointer());
    }

    public void Delete(Action? callback = null, bool unload = false)
    {
        var taskid = StorageFunctions.Delete(new Accessor
        {
            Type = AccessorType.APIDocument,
            Uuid = Metadata.Accessor.Uuid
        }, "dispatch_entry", unload);

        Dispatcher.Register(taskid, callback);
    }

    public void Upload(Action? callback = null, bool unload = false)
    {
        var taskid = StorageFunctions.Upload(new Accessor
        {
            Type = AccessorType.APIDocument,
            Uuid = Metadata.Accessor.Uuid
        }, "dispatch_entry", unload);

        Dispatcher.Register(taskid, callback);
    }

    public async Task EnsureDownloadAsync()
    {
        var tcs = new TaskCompletionSource();
        EnsureDownload(() => tcs.SetResult());
        await tcs.Task;
    }

    public async Task DeleteAsync(bool unload = false)
    {
        var tcs = new TaskCompletionSource();
        Delete(() => tcs.SetResult(), unload);
        await tcs.Task;
    }

    public async Task UploadAsync(bool unload = false)
    {
        var tcs = new TaskCompletionSource();
        Upload(() => tcs.SetResult(), unload);
        await tcs.Task;
    }

    public static TOut Get(string uuid)
    {
        var metadata = Metadata.Get(uuid);

        if (metadata is null) throw new KeyNotFoundException($"No Metadata with uuid {uuid} found");

        var result = new TOut
        {
            Metadata = metadata
        };

        return result;
    }

    /// <summary>
    ///     This function randomizes a lot of the UUIDs of the document,
    ///     it will also update timestamps and set the document to provision.
    /// </summary>
    /// <returns>Returns a new <see cref="Document" /> with the UUID of the new duplicate.</returns>
    public TOut Duplicate()
    {
        var newId = DocumentFunctions.DuplicateDocument(
                InternalFunctions.GetAccessor(Metadata.Accessor.Uuid!).GetPointer())
            .ReadString();

        return Get(newId);
    }

    /// <summary>
    ///     Move the current item to the trash
    /// </summary>
    public void MoveToTrash()
    {
        MoveTo("trash");
    }

    public void MoveTo(string newParent)
    {
        Metadata.Set("parent", newParent);
        Metadata = Metadata.Get(Metadata.Accessor.Uuid!);
    }
}