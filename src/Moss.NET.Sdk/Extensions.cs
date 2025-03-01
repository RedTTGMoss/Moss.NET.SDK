﻿using System;
using System.Text.Json;

namespace Moss.NET.Sdk;

public static class Extensions
{
    public static T GetValue<T>(this JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => (T)(object)element.GetString(),
            JsonValueKind.Number when typeof(T) == typeof(int) => (T)(object)element.GetInt32(),
            JsonValueKind.Number when typeof(T) == typeof(long) => (T)(object)element.GetInt64(),
            JsonValueKind.Number when typeof(T) == typeof(double) => (T)(object)element.GetDouble(),
            JsonValueKind.Number when typeof(T) == typeof(decimal) => (T)(object)element.GetDecimal(),
            JsonValueKind.True or JsonValueKind.False => (T)(object)element.GetBoolean(),
            JsonValueKind.Null => default,
            _ => throw new InvalidOperationException($"Unsupported value kind: {element.ValueKind}")
        };
    }
}
