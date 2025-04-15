using System;

namespace Moss.NET.Sdk.LayoutEngine;

public enum Device
{
    RM1,
    RM2,
    RMPP
}

public static class DeviceExtensions
{
    public static (double width, double height) GetDimension(this Device device, bool isLandscape = true)
    {
        (double width, double height) dimension = device switch
        {
            Device.RM1 => (502.0, 726.0),
            Device.RM2 => (698.0, 533.1),
            Device.RMPP => (777.6, 558.0),
            _ => throw new ArgumentOutOfRangeException(nameof(device), device, null)
        };

        return !isLandscape ? (dimension.height, dimension.width) : dimension;
    }
}