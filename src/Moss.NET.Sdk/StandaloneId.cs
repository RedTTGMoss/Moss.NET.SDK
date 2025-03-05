namespace Moss.NET.Sdk;

public readonly struct StandaloneId(ulong value)
{
    internal ulong Value { get; } = value;

    public static implicit operator ulong(StandaloneId id) => id.Value;
    public static implicit operator StandaloneId(ulong value) => new(value);

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj is StandaloneId id && id.Value == Value;
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(StandaloneId left, StandaloneId right) => left.Value == right.Value;
    public static bool operator !=(StandaloneId left, StandaloneId right) => left.Value != right.Value;
}