using Godot;
using System;

public partial class WanderMode : GameMode
{
  public int PlayerSteps;
  [Export] public int EncounterMaxSteps;
  [Export] public int EncounterRateIncreasePerStep;
  [Export] RichTextLabel DirectionDisplay;

  [Export] public RichTextLabel HPDisplay;
  [Export] public RichTextLabel MPDisplay;
  [Export] Node2D StatDisplay;

  private int PlayerPositionX;
  private int PlayerPositionY;
  private Direction.Facing PlayerFacing;

  public static DungeonFloor CurrentFloor;
  public static int FloorIndex = 0;

  public override void _Ready()
  {
    CurrentFloor = DungeonFloor.Floors[FloorIndex];
    PlayerPositionX = CurrentFloor.StartingX;
    PlayerPositionY = CurrentFloor.StartingY;
    PlayerFacing = CurrentFloor.StartingDir;
    DirectionDisplay.Text = Direction.Chars[(int)PlayerFacing];
    EventBus.Instance.OnMonsterDefeated += OnCombatOver;
    EventBus.Instance.OnUpdateHPMP += OnUpdateHPMP;

    EventBus.Emit(EventBus.SignalName.OnPlayerMove, PlayerPositionX, PlayerPositionY, (int)PlayerFacing);

    PlayerSteps = 0;
  }

  private void CheckTile()
  {
    DungeonTile CurrentTile = (DungeonTile)CurrentFloor.FloorPlan[PlayerPositionX][PlayerPositionY];
    if (CurrentTile == DungeonTile.HEALING_TILE)
    {
      EventBus.Emit(EventBus.SignalName.OnHealingTileEncountered);
    }
    else if (CurrentTile == DungeonTile.LADDER_TILE)
    {
      FloorIndex++;
      CurrentFloor = DungeonFloor.Floors[FloorIndex];
      PlayerPositionX = CurrentFloor.StartingX;
      PlayerPositionY = CurrentFloor.StartingY;
      PlayerFacing = CurrentFloor.StartingDir;
      DirectionDisplay.Text = Direction.Chars[(int)PlayerFacing];
      EventBus.Emit(EventBus.SignalName.OnLadderEncountered);
    }
  }
  public override void ProcessGameMode(double delta)
  {
    if (Input.IsActionJustPressed(InputAction.Stats))
    {
      StatDisplay.Visible = !StatDisplay.Visible;
    }
    if (StatDisplay.Visible)
    {
      return;
    }

    if (Input.IsActionJustPressed(InputAction.MoveForward))
    {
      if (ProcessMoveFoward())
      {
        CheckTile();
        OnMove(PlayerPositionX, PlayerPositionY, (int)PlayerFacing);
        EventBus.Emit(EventBus.SignalName.OnPlayerMove, PlayerPositionX, PlayerPositionY, (int)PlayerFacing);
      }
    }

    if (Input.IsActionJustPressed(InputAction.TurnLeft))
    {
      ProcessTurn(false);
      EventBus.Emit(EventBus.SignalName.OnPlayerMove, PlayerPositionX, PlayerPositionY, (int)PlayerFacing);
      DirectionDisplay.Text = Direction.Chars[(int)PlayerFacing];
    }
    if (Input.IsActionJustPressed(InputAction.TurnRight))
    {
      ProcessTurn(true);
      EventBus.Emit(EventBus.SignalName.OnPlayerMove, PlayerPositionX, PlayerPositionY, (int)PlayerFacing);
      DirectionDisplay.Text = Direction.Chars[(int)PlayerFacing];
    }

    if (Input.IsActionJustPressed(InputAction.TurnAround))
    {
      ProcessTurn(false);
      ProcessTurn(false);
      EventBus.Emit(EventBus.SignalName.OnPlayerMove, PlayerPositionX, PlayerPositionY, (int)PlayerFacing);
    }
  }

  private void ProcessTurn(bool isRight)
  {
    if (isRight)
    {
      PlayerFacing = (Direction.Facing)(((int)PlayerFacing + 1) % 4);
    }
    else
    {
      PlayerFacing = (Direction.Facing)(((int)PlayerFacing + 3) % 4);
    }
  }

  private bool ProcessMoveFoward()
  {
    // validate lack of walls
    switch (PlayerFacing)
    {
      case Direction.Facing.NORTH:
        if (PlayerPositionY == 0) // TODO add map size
        {
          return false;
        }
        if (CurrentFloor.FloorPlan[PlayerPositionX - 1][PlayerPositionY] == (int)DungeonTile.WALL_TILE)
        {
          return false;
        }
        PlayerPositionX -= 1;
        break;
      case Direction.Facing.EAST:
        if (PlayerPositionX > CurrentFloor.FloorPlan[0].Length - 1)
        {
          return false;
        }
        if (CurrentFloor.FloorPlan[PlayerPositionX][PlayerPositionY + 1] == (int)DungeonTile.WALL_TILE)
        {
          return false;
        }
        PlayerPositionY += 1;
        break;
      case Direction.Facing.SOUTH:
        if (PlayerPositionY > CurrentFloor.FloorPlan.Length - 1)
        {
          return false;
        }
        if (CurrentFloor.FloorPlan[PlayerPositionX + 1][PlayerPositionY] == (int)DungeonTile.WALL_TILE)
        {
          return false;
        }
        PlayerPositionX += 1;
        break;
      case Direction.Facing.WEST:
        if (PlayerPositionX == 0)
        {
          return false;
        }
        if (CurrentFloor.FloorPlan[PlayerPositionX][PlayerPositionY - 1] == (int)DungeonTile.WALL_TILE)
        {
          return false;
        }
        PlayerPositionY -= 1;
        break;
    }
    return true;
  }

  public void OnCombatOver(int exp)
  {
    DirectionDisplay.Text = Direction.Chars[(int)PlayerFacing];
  }

  public void OnUpdateHPMP(int hp, int mp)
  {
    HPDisplay.Text = $"{hp}";
    MPDisplay.Text = $"{mp}";
  }

  public void OnRetry()
  {
    CurrentFloor = DungeonFloor.Floors[0];
    PlayerPositionX = CurrentFloor.StartingX;
    PlayerPositionY = CurrentFloor.StartingY;
    PlayerFacing = CurrentFloor.StartingDir;
    DirectionDisplay.Text = Direction.Chars[(int)PlayerFacing];
  }

  private void OnMove(int x, int y, int d)
  {
    PlayerSteps++;
    if (GD.RandRange(0, EncounterMaxSteps) < PlayerSteps)
    {
      PlayerSteps = 0;
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnMonsterEncountered);
    }
  }
}
