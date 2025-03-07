﻿using System.Text.Json.Serialization;
using Moss.NET.Sdk.NEW;
using Moss.NET.Sdk.Storage;

namespace Moss.NET.Sdk.API;

[JsonSerializable(typeof(RMFile))]
[JsonSerializable(typeof(RmPage))]
[JsonSerializable(typeof(RMTimestampedValue))]
[JsonSerializable(typeof(RMCPageUUID))]
[JsonSerializable(typeof(RMCPages))]
[JsonSerializable(typeof(RmZoom))]
[JsonSerializable(typeof(RMZoomMode))]
[JsonSerializable(typeof(RMFileType))]
[JsonSerializable(typeof(RMOrientation))]
[JsonSerializable(typeof(RMTag))]
[JsonSerializable(typeof(Metadata))]
internal partial class ApiJsonContext : JsonSerializerContext
{
}