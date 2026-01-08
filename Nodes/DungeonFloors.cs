using Godot;
using System;

public enum DungeonTile
{
  ERR_TILE = -1,
  OPEN_TILE = 0,
  WALL_TILE = 1,
  LADDER_TILE = 2,
  TREASURE_TILE = 3,
  HEALING_TILE = 4,
}

public class DungeonFloor
{
  public int[][] FloorPlan;
  public int StartingX;
  public int StartingY;
  public Direction.Facing StartingDir;

  public DungeonFloor(int[][] fp, int x, int y, Direction.Facing d)
  {
    FloorPlan = fp;
    StartingX = x;
    StartingY = y;
    StartingDir = d;
  }

  public static DungeonFloor FloorOne = new DungeonFloor(
    [
    [1,1,1,1,1,1,1,1],
    [1,0,0,0,0,0,0,1],
    [1,0,1,1,0,1,4,1],
    [1,0,1,1,0,1,1,1],
    [1,0,1,0,0,0,0,1],
    [1,1,1,1,0,1,0,1],
    [1,2,0,0,0,0,0,1],
    [1,1,1,1,1,1,1,1],
    ],
    1,
    1,
    Direction.Facing.SOUTH
  );

  public static DungeonFloor FloorTwo = new DungeonFloor(
    [
    [1,1,1,1,1,1,1,1,1,1],
    [1,0,0,0,0,0,0,1,2,1],
    [1,1,1,1,0,1,4,1,0,1],
    [1,0,1,0,0,0,1,1,0,1],
    [1,0,0,0,1,0,0,0,0,1],
    [1,1,0,1,1,1,0,1,0,1],
    [1,2,0,1,4,0,0,1,0,1],
    [1,1,1,1,1,1,1,1,1,1],
    ],
    6,
    1,
    Direction.Facing.SOUTH
  );

  public static DungeonFloor[] Floors = [FloorOne, FloorTwo];
}
