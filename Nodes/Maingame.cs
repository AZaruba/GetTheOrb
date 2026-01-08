using Godot;
using System;

public partial class Maingame : Node2D
{
  [Export] Node2D StatDisplay;
  [Export] Dungeon DungeonDisplay;
  [Export] RichTextLabel DirectionDisplay;

  [Export] GameMode WanderMode;
  [Export] GameMode FightMode;
  [Export] GameMode TitleMode;
  [Export] GameMode CharacterSelectMode;
  [Export] TreasureMode TreasureMode;
  [Export] LevelUpMode LevelUpMode;
  [Export] GameOverMode GameOver;

  GameMode StartingGameMode;

  private int PlayerPositionX;
  private int PlayerPositionY;

  private GameMode CurrentGameMode;

  public override void _Ready()
  {
    StartingGameMode = TitleMode;
    CurrentGameMode = StartingGameMode;
    InitSignals();
  }

  public override void _Process(double delta)
  {
    if (Input.IsActionJustPressed("DEBUG_RESET"))
    {
      GetTree().ReloadCurrentScene();
    }
    if (Input.IsActionJustPressed("DEBUG_QUIT"))
    {
      GetTree().Quit();
    }
    CurrentGameMode.ProcessGameMode(delta);
  }

  private void OnMonsterEncountered()
  {
    CurrentGameMode = FightMode;
  }

  private void OnMonsterDefeated(int exp)
  {
    //CurrentGameMode = WanderMode;
  }

  private void OnGameOver()
  {
    GameOver.Visible = true;
    CurrentGameMode = GameOver;
  }

  private void OnRetry()
  {
    GameOver.Visible = false;
    CurrentGameMode = WanderMode;
  }

  private void OnLevelUp(int level, int f, int w, int g)
  {
    if (level == 0)
    {
      return;
    }
    LevelUpMode.Reset();
    LevelUpMode.Visible = true;
    CurrentGameMode = LevelUpMode;
  }

  private void OnLevelUpAdvance()
  {
    LevelUpMode.Visible = false;
    CurrentGameMode = WanderMode;
  }

  private void OnAdvanceFromTitle()
  {
    TitleMode.Visible = false;
    CharacterSelectMode.Visible = true;
    CurrentGameMode = CharacterSelectMode;
  }

  private void OnCharacterSelected(CharacterClass chrClass)
  {
    CharacterSelectMode.Visible = false;
    CurrentGameMode = WanderMode;
  }

  private void OnTreasureFound(int exp, Item item)
  {
    TreasureMode.OnTreasureFound(exp, item);
    CurrentGameMode = TreasureMode;
  }

  private void InitSignals()
  {
    EventBus.Instance.OnMonsterEncountered += OnMonsterEncountered;
    EventBus.Instance.OnMonsterDefeated += OnMonsterDefeated;
    EventBus.Instance.OnAdvanceFromTitle += OnAdvanceFromTitle;
    EventBus.Instance.OnSelectCharacter += OnCharacterSelected;
    EventBus.Instance.OnAdvanceFromLevelUp += OnLevelUpAdvance;
    EventBus.Instance.OnLevelUp += OnLevelUp;
    EventBus.Instance.OnTreasureFound += OnTreasureFound;
    EventBus.Instance.OnGameOver += OnGameOver;
  }
}
