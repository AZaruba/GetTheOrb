using Godot;
using System;

public enum DungeonSpriteFrame
{
  L_WALL_WALL = 0,
  C_OPEN_OPEN,
  R_WALL_WALL,
  L_WALL_OPEN,
  C_OPEN_WALL,
  R_WALL_OPEN,
  L_OPEN_WALL,
  C_CLOSE_WALL,
  R_OPEN_WALL,
  L_CLOSE_WALL,
  C_LADDER,
  R_CLOSE_WALL,
  L_CLOSE_OPEN,
  C_HEALING_CIRCLE,
  R_CLOSE_OPEN,
}

public partial class Dungeon : Node2D
{
  [Export] Sprite2D LeftSprite;
  [Export] Sprite2D CenterSprite;
  [Export] Sprite2D RightSprite;

  [Export] Color[] DungeonColors;

  public override void _Ready()
  {
    EventBus.Instance.OnPlayerMove += OnMove;
    SetDungeonColor(0);
  }

  public override void _ExitTree()
  {
    EventBus.Instance.OnPlayerMove -= OnMove;
  }

  public void SetLeftSprite(DungeonTile near, DungeonTile far, bool isCorner)
  {
    if (far == DungeonTile.ERR_TILE || isCorner)
    {
      if (near == DungeonTile.WALL_TILE)
      {
        LeftSprite.Frame = (int)DungeonSpriteFrame.L_CLOSE_WALL;
      }
      else
      {
        LeftSprite.Frame = (int)DungeonSpriteFrame.L_CLOSE_OPEN;
      }
    }
    else
    {
      if (near == DungeonTile.WALL_TILE)
      {
        if (far == DungeonTile.WALL_TILE)
        {
          LeftSprite.Frame = (int)DungeonSpriteFrame.L_WALL_WALL;
        }
        else
        {
          LeftSprite.Frame = (int)DungeonSpriteFrame.L_WALL_OPEN;
        }
      }
      else
      {
        if (far == DungeonTile.WALL_TILE)
        {
          LeftSprite.Frame = (int)DungeonSpriteFrame.L_OPEN_WALL;
        }
        // open open not needed?
      }
    }
  }
  public void SetCenter(DungeonTile near, DungeonTile far)
  {
    if (near == DungeonTile.WALL_TILE)
    {
      CenterSprite.Frame = (int)DungeonSpriteFrame.C_CLOSE_WALL;
    }
    else if (near == DungeonTile.HEALING_TILE)
    {
      CenterSprite.Frame = (int)DungeonSpriteFrame.C_HEALING_CIRCLE;
    }
    else if (near == DungeonTile.LADDER_TILE || near == DungeonTile.LADDER_UP_TILE)
    {
      CenterSprite.Frame = (int)DungeonSpriteFrame.C_LADDER;
    }
    else
    {
      if (far == DungeonTile.WALL_TILE)
      {
        CenterSprite.Frame = (int)DungeonSpriteFrame.C_OPEN_WALL;
      }
      else
      {
        CenterSprite.Frame = (int)DungeonSpriteFrame.C_OPEN_OPEN;
      }
    }
  }
  public void SetRightSprite(DungeonTile near, DungeonTile far, bool isCorner)
  
  {
    if (far == DungeonTile.ERR_TILE || isCorner)
    {
      if (near == DungeonTile.WALL_TILE)
      {
        RightSprite.Frame = (int)DungeonSpriteFrame.R_CLOSE_WALL;
      }
      else
      {
        RightSprite.Frame = (int)DungeonSpriteFrame.R_CLOSE_OPEN;
      }
    }
    else
    {
      if (near == DungeonTile.WALL_TILE)
      {
        if (far == DungeonTile.WALL_TILE)
        {
          RightSprite.Frame = (int)DungeonSpriteFrame.R_WALL_WALL;
        }
        else
        {
          RightSprite.Frame = (int)DungeonSpriteFrame.R_WALL_OPEN;
        }
      }
      else
      {
        if (far == DungeonTile.WALL_TILE)
        {
          RightSprite.Frame = (int)DungeonSpriteFrame.R_OPEN_WALL;
        }
        // open open not needed?
      }
    }
  }

  public void SetDungeonColor(int FloorIndex)
  {
    LeftSprite.Material.Set("shader_parameter/replace_0", DungeonColors[FloorIndex]);
  }
  public void OnMove(int x, int y, int d)
  {
    // calculate Left Right Center based on position

    DungeonTile cf1 = DungeonTile.ERR_TILE;
    DungeonTile cf2 = DungeonTile.ERR_TILE;
    DungeonTile lf1 = DungeonTile.ERR_TILE;
    DungeonTile lf2 = DungeonTile.ERR_TILE;
    DungeonTile rf1 = DungeonTile.ERR_TILE;
    DungeonTile rf2 = DungeonTile.ERR_TILE;

    switch((Direction.Facing)d)
    {
      case Direction.Facing.NORTH:
        cf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-1][y];
        lf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y-1];
        rf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y+1];
        if (x > 1)
        {
          cf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-2][y];
          lf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-1][y-1];
          rf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-1][y+1];
        }
        break;
      case Direction.Facing.EAST:
        cf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y+1];
        lf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-1][y];
        rf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+1][y];
        if (y < WanderMode.CurrentFloor.FloorPlan[0].Length - 2)
        {
          cf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y+2];
          lf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-1][y+1];
          rf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+1][y+1];
        }
        break;
      case Direction.Facing.SOUTH:
        cf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+1][y];
        lf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y+1];
        rf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y-1];
        if (x < WanderMode.CurrentFloor.FloorPlan.Length - 2)
        {
          cf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+2][y];
          lf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+1][y+1];
          rf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+1][y-1];
        }
        break;
      case Direction.Facing.WEST:
        cf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y-1];
        lf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+1][y];
        rf1 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-1][y];
        if (y > 1)
        {
          cf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x][y-2];
          lf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x+1][y-1];
          rf2 = (DungeonTile)WanderMode.CurrentFloor.FloorPlan[x-1][y-1];
        }
        break;
    }

    SetCenter(cf1, cf2);
    SetLeftSprite(lf1, lf2, cf1 == DungeonTile.WALL_TILE);
    SetRightSprite(rf1, rf2, cf1 == DungeonTile.WALL_TILE);
  }
}
