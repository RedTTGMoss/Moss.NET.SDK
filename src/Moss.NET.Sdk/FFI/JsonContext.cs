using System.Collections.Generic;
using System.Text.Json.Serialization;
using Moss.NET.Sdk.FFI.Dto;
using Moss.NET.Sdk.NEW;
using Moss.NET.Sdk.Scheduler;
using Moss.NET.Sdk.Storage;
using Moss.NET.Sdk.UI;
using Screen = Moss.NET.Sdk.FFI.Dto.Screen;

namespace Moss.NET.Sdk.FFI;

[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(Color))]
[JsonSerializable(typeof(TextColor))]
[JsonSerializable(typeof(ContextButton))]
[JsonSerializable(typeof(ContextMenu))]
[JsonSerializable(typeof(List<ContextButton>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(List<ConfigGetD>))]
[JsonSerializable(typeof(List<ConfigGetS>))]
[JsonSerializable(typeof(Accessor[]))]
[JsonSerializable(typeof(MossState))]
[JsonSerializable(typeof(ExtensionInfo))]
[JsonSerializable(typeof(ConfigSet))]
[JsonSerializable(typeof(ConfigGetS))]
[JsonSerializable(typeof(ConfigGetD))]
[JsonSerializable(typeof(PygameExtraRect))]
[JsonSerializable(typeof(PygameExtraRectEdgeRounding))]
[JsonSerializable(typeof(Rect))]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(Screen))]
[JsonSerializable(typeof(Accessor))]
[JsonSerializable(typeof(DocumentNewNotebook))]
[JsonSerializable(typeof(DocumentNewPdf))]
[JsonSerializable(typeof(DocumentNewEpub))]
[JsonSerializable(typeof(MetadataNew))]
[JsonSerializable(typeof(RmPage))]
[JsonSerializable(typeof(RMDocumentType))]
[JsonSerializable(typeof(Metadata))]
[JsonSerializable(typeof(RootInfo))]
[JsonSerializable(typeof(FileSyncProgress))]
[JsonSerializable(typeof(DocumentSyncProgress))]
[JsonSerializable(typeof(RmFileList))]
[JsonSerializable(typeof(List<ScheduledTask>))]
internal partial class JsonContext : JsonSerializerContext
{
}