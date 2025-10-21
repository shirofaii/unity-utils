using System;
// ReSharper disable InconsistentNaming

[Flags] public enum Direction : byte {
  None = 0,
  N = 1 << 0,
  NE = 1 << 1,
  E = 1 << 2,
  SE = 1 << 3,
  S = 1 << 4,
  SW = 1 << 5,
  W = 1 << 6,
  NW = 1 << 7,
  All = byte.MaxValue,
}

public static class DirectionHelper {
  public static readonly Direction[] All = {
    Direction.N,
    Direction.NE,
    Direction.E,
    Direction.SE,
    Direction.S,
    Direction.SW,
    Direction.W,
    Direction.NW,
  }; 
  
  public static Direction Opposite(this Direction direction) {
    return direction switch {
      Direction.N => Direction.S,
      Direction.NE => Direction.SW,
      Direction.E => Direction.W,
      Direction.SE => Direction.NW,
      Direction.S => Direction.N,
      Direction.SW => Direction.NE,
      Direction.W => Direction.E,
      Direction.NW => Direction.SE,
      
      Direction.None => Direction.None,
      Direction.All => Direction.All,
      _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
  }

  public static void ForEach(this Direction direction, Action<Direction> action) {
    if (direction.HasFlag(Direction.N)) { action.Invoke(Direction.N); }
    if (direction.HasFlag(Direction.NE)) { action.Invoke(Direction.NE); }
    if (direction.HasFlag(Direction.E)) { action.Invoke(Direction.E); }
    if (direction.HasFlag(Direction.SE)) { action.Invoke(Direction.SE); }
    if (direction.HasFlag(Direction.S)) { action.Invoke(Direction.S); }
    if (direction.HasFlag(Direction.SW)) { action.Invoke(Direction.SW); }
    if (direction.HasFlag(Direction.W)) { action.Invoke(Direction.W); }
    if (direction.HasFlag(Direction.NW)) { action.Invoke(Direction.NW); }
  }
}