namespace AoC.Library.Utils;

public struct Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public bool InBounds(int width, int height) => X >= 0 && X < width && Y >= 0 && Y < height;

    public Point Loop(int width, int height) => new(
        (X + width * (Math.Abs(X) / width + 3)) % width,
        (Y + height * (Math.Abs(Y) / height + 3)) % height
    );

    public static Point Up => new(0, -1);

    public static Point Right => new(1, 0);

    public static Point Down => new(0, 1);

    public static Point Left => new(-1, 0);

    public static Point Zero => new(0, 0);

    public static implicit operator (int, int)(Point p) => (p.X, p.Y);

    public static implicit operator Point((int, int) tuple) => new(tuple.Item1, tuple.Item2);

    public static implicit operator Point(Direction dir) => dir switch {
        Direction.Up => Up,
        Direction.Right => Right,
        Direction.Down => Down,
        Direction.Left => Left,
        _ => Zero
    };

    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);

    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);

    public static Point operator *(Point a, int v) => new(a.X * v, a.Y * v);
}