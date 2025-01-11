﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moss.NET.Sdk;

[JsonSerializable(typeof(Color))]
[JsonSerializable(typeof(ContextButton))]
[JsonSerializable(typeof(List<ContextButton>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(List<ConfigGet>))]
[JsonSerializable(typeof(Text))]
[JsonSerializable(typeof(MossState))]
[JsonSerializable(typeof(ExtensionInfo))]
public partial class JsonContext : JsonSerializerContext {}