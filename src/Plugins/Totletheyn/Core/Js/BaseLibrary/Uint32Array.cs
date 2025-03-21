using System;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Core.Interop;

namespace Totletheyn.Core.Js.BaseLibrary;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Uint32Array : TypedArray
{
    public Uint32Array()
    {
    }

    public Uint32Array(int length)
        : base(length)
    {
    }

    public Uint32Array(ArrayBuffer buffer)
        : base(buffer, 0, buffer.byteLength)
    {
    }

    public Uint32Array(ArrayBuffer buffer, int bytesOffset)
        : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Uint32Array(ArrayBuffer buffer, int bytesOffset, int length)
        : base(buffer, bytesOffset, length)
    {
    }

    public Uint32Array(JSValue src)
        : base(src)
    {
    }

    protected override JSValue this[int index]
    {
        get
        {
            var res = new Element(this, index);
            res._iValue = (int)getValue(index);
            if (res._iValue >= 0)
            {
                res._valueType = JSValueType.Integer;
            }
            else
            {
                res._dValue = (uint)res._iValue;
                res._valueType = JSValueType.Double;
            }

            return res;
        }
        set
        {
            if (index < 0 || index > length._iValue)
                ExceptionHelper.Throw(new RangeError());
            var v = Tools.JSObjectToInt32(value, 0, false);
            buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 0] = (byte)v;
            buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 1] = (byte)(v >> 8);
            buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 2] = (byte)(v >> 16);
            buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 3] = (byte)(v >> 24);
        }
    }

    public override int BYTES_PER_ELEMENT => sizeof(uint);

    [Hidden]
    public override Type ElementType
    {
        [Hidden] get => typeof(uint);
    }

    private uint getValue(int index)
    {
        return (uint)(buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 0]
                      | (buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 1] << 8)
                      | (buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 2] << 16)
                      | (buffer.data[index * BYTES_PER_ELEMENT + byteOffset + 3] << 24));
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args)
    {
        return subarrayImpl<Uint32Array>(args[0], args[1]);
    }

    protected internal override System.Array ToNativeArray()
    {
        var res = new uint[length._iValue];
        for (var i = 0; i < res.Length; i++)
            res[i] = getValue(i);
        return res;
    }
}