using System;

public static class DirectionHelper
{
    public static Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static (int x, int y) GetOffset(Direction direction)
    {
        return direction switch
        {
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            Direction.Up => (0, 1),
            Direction.Down => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };
    }
}