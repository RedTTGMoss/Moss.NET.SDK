//ToDo: make all edgerounding properties optional

namespace Moss.NET.Sdk.FFI.Dto;

public record PygameExtraRectEdgeRounding(
    int edge_rounding,
    int edge_rounding_topright,
    int edge_rounding_topleft,
    int edge_rounding_bottomright,
    int edge_rounding_bottomleft);