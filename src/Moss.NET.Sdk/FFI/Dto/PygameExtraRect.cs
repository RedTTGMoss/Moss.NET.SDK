using Moss.NET.Sdk.UI;

namespace Moss.NET.Sdk.FFI.Dto;

internal record PygameExtraRect(Color color, Rect rect, int width, PygameExtraRectEdgeRounding? edge_rounding = null);