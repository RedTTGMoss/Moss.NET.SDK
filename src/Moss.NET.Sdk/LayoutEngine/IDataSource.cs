using System.Xml.Linq;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine;

public interface IDataSource
{
    string Name { get; }
    void ApplyData(YogaNode node, PdfPageBuilder page, XElement element);
}