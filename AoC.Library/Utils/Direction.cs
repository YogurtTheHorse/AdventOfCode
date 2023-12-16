namespace AoC.Library.Utils;

public enum Direction
{
    None,
    
    Up,
    Right,
    Down,
    Left
}

public static class DirectionUtils
{
    public static char AsAscii(this Direction d) => d switch {
        Direction.Up => '^',
        Direction.Right => '>',
        Direction.Down => 'v',
        Direction.Left => '<',
        _ => 'O'
    };
    
    public static bool IsHorizontal(this Direction d) => d is Direction.Left or Direction.Right;

    public static bool IsVertical(this Direction d) => d is Direction.Up or Direction.Down;

    public static Direction Opposite(this Direction d) => d switch {
        Direction.Up => Direction.Down,
        Direction.Right => Direction.Left,
        Direction.Down => Direction.Up,
        Direction.Left => Direction.Right,
        _ => Direction.None
    };
}