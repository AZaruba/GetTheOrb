using Godot;
using System;

public partial class TreasureMode : GameMode
{
  [Export] Sprite2D TreasureSprite;
  [Export] Sprite2D ItemSprite;
  [Export] InspectLayer InspectLayer;
  bool TreasureOpened = false;

  Item FoundItem;

  int MonsterEXP = 0;

  public override void _Ready()
  {
    // no need to subscribe as main game will populate
  }
  public override void ProcessGameMode(double delta)
  {
    if (!TreasureOpened && Input.IsActionJustPressed(InputAction.Advance))
    {
      TreasureSprite.Visible = false;
      ItemSprite.Visible = true;
      TreasureOpened = true;
    }
    if (TreasureOpened)
    {
      if (Input.IsActionJustPressed(InputAction.Inspect))
      {
        InspectLayer.Visible = !InspectLayer.Visible;
      }
      if (Input.IsActionJustPressed(InputAction.LeftHand))
      {
        Visible = false;
        EventBus.Emit(EventBus.SignalName.OnPickupItem, 1, FoundItem);
        EventBus.Emit(EventBus.SignalName.OnMonsterDefeated, MonsterEXP);
      }
      else if (Input.IsActionJustPressed(InputAction.RightHand))
      {
        Visible = false;
        EventBus.Emit(EventBus.SignalName.OnPickupItem, 2, FoundItem);
        EventBus.Emit(EventBus.SignalName.OnMonsterDefeated, MonsterEXP);
      }
    }
    
    // can drop loot even if you haven't seen it
    if (Input.IsActionJustPressed(InputAction.Drop))
    {
        Visible = false;
        EventBus.Emit(EventBus.SignalName.OnMonsterDefeated, MonsterEXP);
    }
  }

  public void OnTreasureFound(int exp, Item item)
  {
    Visible = true;
    MonsterEXP = exp;
    FoundItem = item;
    ItemSprite.Texture = FoundItem.Sprite;
    InspectLayer.Populate(item.Fit, item.Wit, item.Grit);
  }
}
