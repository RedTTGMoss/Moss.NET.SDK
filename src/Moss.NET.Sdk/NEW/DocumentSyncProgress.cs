using System.Text.Json.Serialization;

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