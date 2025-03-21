using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Float64Array : TypedArray
{
    public Float64Array()
    {
    }

    public Float64Array(int length)
        : base(length)
    {
    }

    public Float64Array(ArrayBuffer buffer)
        : base(buffer, 0, buffer.byteLength)
    {
    }

    public Float64Array(ArrayBuffer buffer, int bytesOffset)
        : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Float64Array(ArrayBuffer buffer, int bytesOffset, int length)
        : base(buffer, bytesOffset, length)
    {
    }

    public Float64Array(JSValue src)
        : base(src)
    {
    }

    protected override JSValue this[int index]
    {
        get
        {
            var res = new Element(this, index);
            res._dValue = getValue(index);
            res._valueType = JSValueType.Double;
            return res;
        }
        set
        {
            if (index < 0 || index > length._iValue)
                ExceptionHelper.Throw(new RangeError());

            var v = BitConverter.DoubleToInt64Bits(Tools.JSObjectToDouble(value));
            var byteIndex = index * BYTES_PER_ELEMENT + byteOffset;
            buffer.data[byteIndex + 0] = (byte)(v >> 0);
            buffer.data[byteIndex + 1] = (byte)(v >> 8);
            buffer.data[byteIndex + 2] = (byte)(v >> 16);
            buffer.data[byteIndex + 3] = (byte)(v >> 24);
            buffer.data[byteIndex + 4] = (byte)(v >> 32);
            buffer.data[byteIndex + 5] = (byte)(v >> 40);
            buffer.data[byteIndex + 6] = (byte)(v >> 48);
            buffer.data[byteIndex + 7] = (byte)(v >> 56);
        }
    }

    public override int BYTES_PER_ELEMENT => sizeof(double);

    [Hidden]
    public override Type ElementType
    {
        [Hidden] get => typeof(double);
    }

    private double getValue(int index)
    {
        return BitConverter.ToDouble(buffer.data, index * BYTES_PER_ELEMENT + byteOffset);
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args)
    {
        return subarrayImpl<Float64Array>(args[0], args[1]);
    }

    protected internal override System.Array ToNativeArray()
    {
        var res = new double[length._iValue];
        for (var i = 0; i < res.Length; i++)
            res[i] = getValue(i);
        return res;
    }
}