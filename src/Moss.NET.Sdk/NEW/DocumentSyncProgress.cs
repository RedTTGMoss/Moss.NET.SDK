using System.Text.Json.Serialization;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Moss.NET.Sdk.NEW;

public class DocumentSyncProgress : FileSyncProgress
{
    [JsonPropertyName("document_uuid")]
    public string DocumentUuid { get; set; }

    [JsonPropertyName("file_sync_operation")]
    public FileSyncProgress FileSyncOperation { get; set; }

    [JsonPropertyName("total_tasks")]
    public ulong TotalTasks { get; set; }

    [JsonPropertyName("finished_tasks")]
    public ulong FinishedTasks { get; set; }

    [JsonPropertyName("_tasks_was_set_once")]
    public bool TasksWasSetOnce { get; set; }
}