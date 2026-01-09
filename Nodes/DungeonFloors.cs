using Godot;
using System;

public enum DungeonTile
{
  ERR_TILE = -1,
  OPEN_TILE = 0,
  WALL_TILE = 1,
  LADDER_TILE = 2,
  LADDER_UP_TILE = 3,
  TREASURE_TILE = 4,
  HEALING_TILE = 5,
}

public class DungeonFloor
{
  public int[][] FloorPlan;
  public int StartingX;
  public int StartingY;

  public int LadderX;
  public int LadderY;
  public Direction.Facing StartingDir;

  public DungeonFloor(int[][] fp, int x, int y, Direction.Facing d, int lx, int ly)
  {
    FloorPlan = fp;
    StartingX = x;
    StartingY = y;
    StartingDir = d;
    LadderX = lx;
    LadderY = ly;
  }

  public static DungeonFloor FloorOne = new DungeonFloor(
    [
    [1,1,1,1,1,1,1,1],
    [1,0,0,0,0,0,0,1],
    [1,0,1,1,0,1,5,1],
    [1,0,1,1,0,1,1,1],
    [1,0,1,0,0,0,0,1],
    [1,1,1,1,0,1,0,1],
    [1,2,0,0,0,0,0,1],
    [1,1,1,1,1,1,1,1],
    ],
    1,
    1,
    Direction.Facing.SOUTH,
    6,
    1
  );

  public static DungeonFloor FloorTwo = new DungeonFloor(
    [
    [1,1,1,1,1,1,1,1,1,1],
    [1,0,0,0,0,0,0,1,2,1],
    [1,1,1,1,0,1,5,1,0,1],
    [1,0,1,0,0,0,1,1,0,1],
    [1,0,0,0,1,0,0,0,0,1],
    [1,1,0,1,1,1,0,1,0,1],
    [1,3,0,1,4,0,0,1,0,1],
    [1,1,1,1,1,1,1,1,1,1],
    ],
    6,
    1,
    Direction.Facing.EAST,
    1,
    8
  );
  
  public static DungeonFloor FloorThree = new DungeonFloor(
    [
    [1,1,1],
    [1,3,1],
    [1,0,1],
    [1,0,1],
    [1,0,1],
    [1,0,1],
    [1,0,1],
    [1,1,1],
    ],
    1,
    1,
    Direction.Facing.SOUTH,
    1,
    1
  );

  public static DungeonFloor[] Floors = [FloorOne, FloorTwo, FloorThree];
}
