using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Uint8ClampedArray : TypedArray
{
    public Uint8ClampedArray()
    {
    }

    public Uint8ClampedArray(int length)
        : base(length)
    {
    }

    public Uint8ClampedArray(ArrayBuffer buffer)
        : base(buffer, 0, buffer.byteLength)
    {
    }

    public Uint8ClampedArray(ArrayBuffer buffer, int bytesOffset)
        : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Uint8ClampedArray(ArrayBuffer buffer, int bytesOffset, int length)
        : base(buffer, bytesOffset, length)
    {
    }

    public Uint8ClampedArray(JSValue src)
        : base(src)
    {
    }

    protected override JSValue this[int index]
    {
        get
        {
            var res = new Element(this, index);
            res._iValue = getValue(index);
            res._valueType = JSValueType.Integer;
            return res;
        }
        set
        {
            if (index < 0 || index > length._iValue)
                ExceptionHelper.Throw(new RangeError());
            buffer.data[index + byteOffset] =
                (byte)System.Math.Min(255, System.Math.Max(0, Tools.JSObjectToInt32(value, 0, false)));
        }
    }

    public override int BYTES_PER_ELEMENT => sizeof(byte);

    [Hidden]
    public override Type ElementType
    {
        [Hidden] get => typeof(byte);
    }

    private byte getValue(int index)
    {
        return buffer.data[index + byteOffset];
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args)
    {
        return subarrayImpl<Uint8ClampedArray>(args[0], args[1]);
    }

    protected internal override System.Array ToNativeArray()
    {
        var res = new byte[length._iValue];
        for (var i = 0; i < res.Length; i++)
            res[i] = getValue(i);
        return res;
    }
}