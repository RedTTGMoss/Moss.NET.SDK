using System;
using PolyType;

namespace Totletheyn.Core;

[GenerateShape]
public partial record Issue(string Title, string PdfUrl, DateTime PublishingDate);