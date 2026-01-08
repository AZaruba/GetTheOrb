
using Godot;

public partial class EventBus : Node
{
  public static EventBus Instance;

  [Signal] public delegate void OnPlayerMoveEventHandler(int x, int y, int d);
  [Signal] public delegate void OnMonsterEncounteredEventHandler();
  [Signal] public delegate void OnMonsterDefeatedEventHandler(int exp);
  [Signal] public delegate void OnLevelUpEventHandler(int level, int f, int w, int g);
  [Signal] public delegate void OnUpdateHPMPEventHandler(int hp, int mp);


  [Signal] public delegate void OnLadderEncounteredEventHandler();
  [Signal] public delegate void OnHealingTileEncounteredEventHandler();
  [Signal] public delegate void OnGameOverEventHandler();
  [Signal] public delegate void OnRetryEventHandler();
  [Signal] public delegate void OnPickupItemEventHandler(int slot, Item item);
  [Signal] public delegate void OnTreasureFoundEventHandler(int exp, Item item);
  [Signal] public delegate void OnAdvanceFromTitleEventHandler();
  [Signal] public delegate void OnAdvanceFromLevelUpEventHandler();
  [Signal] public delegate void OnSelectCharacterEventHandler(CharacterClass chrClass);

  public EventBus()
  {
    Instance = this;
  }

  public static void Emit(string SignalName)
  {
    Instance.EmitSignal(SignalName);
  }

  public static void Emit(string SignalName, int x)
  {
    Instance.EmitSignal(SignalName, x);
  }

  public static void Emit(string SignalName, CharacterClass chrClass)
  {
    Instance.EmitSignal(SignalName, chrClass);
  }

  public static void Emit(string SignalName, int slot, Item item)
  {
    Instance.EmitSignal(SignalName, slot, item);
  }
  public static void Emit(string SignalName, int x, int y)
  {
    Instance.EmitSignal(SignalName, x, y);
  }

  public static void Emit(string SignalName, int x, int y, int d)
  {
    Instance.EmitSignal(SignalName, x, y, d);
  }
}