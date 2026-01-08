using Godot;
using Godot.Collections;
using System;

public partial class Player : Node2D
{
  public CharacterClass Job;
  public Array<int> LevelCurve = [0,8,20,36,60,108,172,256,356,900];

  [Export] public Sprite2D CharSprite;
  [Export] public Sprite2D LeftHandSprite;
  [Export] public Sprite2D RightHandSprite;

  [Export] public StatOverlay StatsDisplay;

  public int CurrentHP;
  public int CurrentMP;

  public Item LeftHandItem;
  public Item RightHandItem;

  public int EXP = 0;
  public int Level = 1;
  public int MaxLevel = 9;

  public override void _Ready()
  {
    EventBus.Instance.OnMonsterDefeated += OnMonsterDefeated;
    EventBus.Instance.OnPickupItem += EquipItem;
    EventBus.Instance.OnSelectCharacter += OnCharacterSelected;
    EventBus.Instance.OnHealingTileEncountered += OnHealingTileEncounter;
    EventBus.Instance.OnRetry += OnRetry;
  }

  public void OnCharacterSelected(CharacterClass chrClass)
  {
    Job = chrClass;
    Job.Wit = Job.BaseWit;
    Job.Fit = Job.BaseFit;
    Job.Grit = Job.BaseGrit;
    CharSprite.Texture = Job.Sprite;

    CurrentHP = Job.BaseGrit;
    CurrentMP = Job.BaseWit;
    EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);

    LeftHandItem = ResourceLoader.Load<Item>("res://Assets/Data/Items/Stick.tres");
    RightHandItem = ResourceLoader.Load<Item>("res://Assets/Data/Items/NULL.tres");
    LeftHandSprite.Texture = LeftHandItem.Sprite;
    RightHandSprite.Texture = RightHandItem.Sprite;
  }

  public int GetPrimaryStat(bool isLeft)
  {
    if (Job.PrimaryStat == CharacterClass.Stat.WIT)
    {
      return Job.Wit + (isLeft ? LeftHandItem.Wit : RightHandItem.Wit);
    }
    else if (Job.PrimaryStat == CharacterClass.Stat.FIT)
    {
      return Job.Fit + (isLeft ? LeftHandItem.Fit : RightHandItem.Fit);
    }
    else
    {
      return Job.Grit + (isLeft ? LeftHandItem.Grit : RightHandItem.Grit);
    }
  }

  public void OnHealingTileEncounter()
  {
    CurrentHP = Job.Grit;
    CurrentMP = Job.Wit;
    EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);
  }

  public void OnMonsterDefeated(int exp)
  {
    EXP += exp;
    if (EXP >= LevelCurve[Level] && Level < MaxLevel)
    {
      Level++;
      Job.Fit += Job.FitOnLevel[Level];
      Job.Wit += Job.WitOnLevel[Level];
      Job.Grit += Job.GritOnLevel[Level];

      CurrentHP = Job.Grit;
      CurrentMP = Job.Wit;
      EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnLevelUp, Level, Job.Fit, Job.Wit, Job.Grit);
    } else
    {
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnAdvanceFromLevelUp);
    }
  }

  public void EquipItem(int slot, Item item)
  {
    if (slot == 1)
    {
      LeftHandItem = item;
      LeftHandSprite.Texture = LeftHandItem.Sprite;
    }
    if (slot == 2)
    {
      RightHandItem = item;
      RightHandSprite.Texture = RightHandItem.Sprite;
    }
  }

  private void OnRetry()
  {
    CurrentHP = Job.Grit;
    CurrentMP = Job.Wit;
    EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);
  }
}
