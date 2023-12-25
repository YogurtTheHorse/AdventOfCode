using System.Numerics;

namespace AoC.Library.Utils;

public struct PointBase<T> : IEquatable<PointBase<T>> where T : INumber<T>
{
    public T X { get; set; }

    public T Y { get; set; }

    public PointBase(T x, T y)
    {
        X = x;
        Y = y;
    }

    public PointBase(IList<T> c) => (X, Y) = c.Unpack2();

    public override string ToString() => $"({X}, {Y})";

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public bool InBounds(T width, T height) => X >= T.Zero && X < width && Y >= T.Zero && Y < height;

    public PointBase<T> Loop(T width, T height) => new(
        (X + width * (T.Abs(X) / width)) % width,
        (Y + height * (T.Abs(Y) / height)) % height
    );

    public static (PointBase<T>, PointBase<T>) Bounds(PointBase<T> p1, PointBase<T> p2) => (Min(p1, p2), Max(p1, p2));

    public static PointBase<T> Min(PointBase<T> p1, PointBase<T> p2) => new(T.Min(p1.X, p2.X), T.Min(p1.Y, p2.Y));

    public static PointBase<T> Max(PointBase<T> p1, PointBase<T> p2) => new(T.Max(p1.X, p2.X), T.Max(p1.Y, p2.Y));

    public static PointBase<T> Up { get; } = new(T.Zero, -T.One);

    public static PointBase<T> Right { get; } = new(T.One, T.Zero);

    public static PointBase<T> Down { get; } = new(T.Zero, T.One);

    public static PointBase<T> Left { get; } = new(-T.One, T.Zero);

    public static PointBase<T> Zero { get; } = new(T.Zero, T.Zero);

    public static implicit operator (T, T)(PointBase<T> p) => (p.X, p.Y);

    public static implicit operator PointBase<T>((T, T) tuple) => new(tuple.Item1, tuple.Item2);

    public static implicit operator PointBase<T>(Direction dir) => dir switch {
        Direction.Up => Up,
        Direction.Right => Right,
        Direction.Down => Down,
        Direction.Left => Left,
        _ => Zero
    };

    public static PointBase<T> operator +(PointBase<T> a, PointBase<T> b) => new(a.X + b.X, a.Y + b.Y);

    public static PointBase<T> operator -(PointBase<T> a, PointBase<T> b) => new(a.X - b.X, a.Y - b.Y);

    public static PointBase<T> operator *(PointBase<T> a, T v) => new(a.X * v, a.Y * v);

    public static bool operator ==(PointBase<T> a, PointBase<T> b) => a.Equals(b);

    public static bool operator !=(PointBase<T> a, PointBase<T> b) => !a.Equals(b);

    public bool Equals(PointBase<T> other) =>
        EqualityComparer<T>.Default.Equals(X, other.X)
        && EqualityComparer<T>.Default.Equals(Y, other.Y);

    public override bool Equals(object? obj) => obj is PointBase<T> other && Equals(other);
}