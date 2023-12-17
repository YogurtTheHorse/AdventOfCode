namespace AoC.Library.Utils;

public record Point(int X, int Y)
{
    public bool InBounds(int width, int height) => X >= 0 && X < width && Y >= 0 && Y < height;

    public Point Loop(int width, int height) => new Point(
        (X + width) % width,
        (Y + height) % height
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
}