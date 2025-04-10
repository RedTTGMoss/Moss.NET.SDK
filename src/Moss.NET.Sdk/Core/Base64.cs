using System;
using System.IO;
using Extism;

namespace Moss.NET.Sdk.Core;

public class Base64 : Stream
{
    private readonly MemoryStream strm = new();

    public Base64()
    {
    }

    public Base64(Stream stream)
    {
        stream.CopyTo(strm);
    }

    public Base64(byte[] bytes)
    {
        strm = new MemoryStream(bytes);
    }

    public Base64(string base64)
    {
        strm = new MemoryStream(Convert.FromBase64String(base64));
    }

    public override bool CanRead => strm.CanRead;
    public override bool CanSeek => strm.CanSeek;
    public override bool CanWrite => strm.CanWrite;
    public override long Length => strm.Length;

    public override long Position
    {
        get => strm.Position;
        set => strm.Position = value;
    }

    public override void Flush()
    {
        strm.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return strm.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return strm.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        strm.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        strm.Write(buffer, offset, count);
    }

    public static implicit operator Base64(MemoryStream ms)
    {
        return new Base64(ms);
    }

    public static implicit operator Base64(MemoryBlock block)
    {
        return new Base64(block.ReadBytes());
    }

    public static implicit operator Base64(byte[] bytes)
    {
        return new Base64(bytes);
    }

    public override string ToString()
    {
        return Convert.ToBase64String(strm.ToArray());
    }

    public string Decode() {
        return new StreamReader(strm).ReadToEnd();
    }
}