using System;

namespace Moss.NET.Sdk.Formats.Core;

public class EpubException(string message) : Exception(message)
{
}

public class EpubParseException(string message) : EpubException($"EPUB parsing error: {message}")
{
}

public class EpubWriteException(string message) : EpubException($"EPUB write error: {message}")
{
}