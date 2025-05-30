﻿using System.Xml.Linq;

namespace Moss.NET.Sdk.Formats.Epub.Extensions;

internal static class XmlExt
{
    public static IList<string> AsStringList(this IEnumerable<XElement> self)
    {
        return self.Select(elem => elem.Value).ToList();
    }

    public static IList<T> AsObjectList<T>(this IEnumerable<XElement> self, Func<XElement, T> factory)
    {
        return self.Select(factory).Where(value => value != null).ToList();
    }
}