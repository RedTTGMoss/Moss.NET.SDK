﻿namespace Totletheyn.Core.RSS.Feeds.MediaRSS;

/// <summary>
///     Specifies the type of an object
/// </summary>
public enum Medium
{
    /// <summary>
    ///     Image
    /// </summary>
    Image,

    /// <summary>
    ///     Audio
    /// </summary>
    Audio,

    /// <summary>
    ///     Video
    /// </summary>
    Video,

    /// <summary>
    ///     Document
    /// </summary>
    Document,

    /// <summary>
    ///     Executable
    /// </summary>
    Executable,

    /// <summary>
    ///     Type could not be determined
    /// </summary>
    Unknown
}