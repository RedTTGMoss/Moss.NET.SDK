namespace Moss.NET.Sdk;

public readonly struct StandaloneId(ulong value)
{
    internal ulong Value { get; } = value;

    public static implicit operator ulong(StandaloneId id)
    {
        return id.Value;
    }

    public static implicit operator StandaloneId(ulong value)
    {
        return new StandaloneId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override bool Equals(object? obj)
    {
        return obj is StandaloneId id && id.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(StandaloneId left, StandaloneId right)
    {
        return left.Value == right.Value;
    }

    public static bool operator !=(StandaloneId left, StandaloneId right)
    {
        return left.Value != right.Value;
    }
}