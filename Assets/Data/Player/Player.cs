using Godot;
using Godot.Collections;
using System;

public partial class Player : Node2D
{
  public CharacterClass Job;
  public Array<int> LevelCurve = [12,36,108,256,512,900,1800,3200,6400];

  [Export] public Sprite2D CharSprite;
  [Export] public Sprite2D LeftHandSprite;
  [Export] public Sprite2D RightHandSprite;

  [Export] public StatOverlay StatsDisplay;

  public int CurrentHP;
  public int CurrentMP;

  public Item LeftHandItem;
  public Item RightHandItem;

  public int EXP = 0;
  public int Level = 0; //1
  public int MaxLevel = 8;//9

  public override void _Ready()
  {
    EventBus.Instance.OnMonsterDefeated += OnMonsterDefeated;
    EventBus.Instance.OnPickupItem += EquipItem;
    EventBus.Instance.OnSelectCharacter += OnCharacterSelected;
    EventBus.Instance.OnHealingTileEncountered += OnHealingTileEncounter;
    EventBus.Instance.OnRetry += OnRetry;
  }

  public override void _ExitTree()
  {
    EventBus.Instance.OnMonsterDefeated -= OnMonsterDefeated;
    EventBus.Instance.OnPickupItem -= EquipItem;
    EventBus.Instance.OnSelectCharacter -= OnCharacterSelected;
    EventBus.Instance.OnHealingTileEncountered -= OnHealingTileEncounter;
    EventBus.Instance.OnRetry -= OnRetry;
  }

  public void OnCharacterSelected(CharacterClass chrClass)
  {
    Job = chrClass;
    Job.Wit = Job.BaseWit;
    Job.Fit = Job.BaseFit;
    Job.Grit = Job.BaseGrit;
    CharSprite.Texture = Job.Sprite;

    CurrentHP = Job.HPOnLevel[Level];
    CurrentMP = Job.BaseWit;
    EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);

    RightHandItem = ResourceLoader.Load<Item>("res://Assets/Data/Items/Stick.tres");
    LeftHandItem = ResourceLoader.Load<Item>("res://Assets/Data/Items/NULL.tres");
    RightHandSprite.Texture = RightHandItem.Sprite;
    LeftHandSprite.Texture = LeftHandItem.Sprite;
  }

  public int GetPrimaryStat(bool isLeft)
  {
    if (Job.PrimaryStat == CharacterClass.Stat.WIT)
    {
      return Job.Wit;
    }
    else if (Job.PrimaryStat == CharacterClass.Stat.FIT)
    {
      return Job.Fit;
    }
    else
    {
      return Job.Grit;
    }
  }

  public void OnHealingTileEncounter()
  {
    CurrentHP = Job.HPOnLevel[Level];
    CurrentMP = Job.Wit;
    EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);
  }

  public void OnMonsterDefeated(int exp)
  {
    EXP += exp;
    if (EXP >= LevelCurve[Level] && Level < MaxLevel)
    {
      Level++;
      GD.Print(Job.GritOnLevel[Level]);
      Job.Fit = Job.FitOnLevel[Level];
      Job.Wit = Job.WitOnLevel[Level];
      Job.Grit = Job.GritOnLevel[Level];

      CurrentHP = Job.HPOnLevel[Level];
      CurrentMP = Job.Wit;
      EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnLevelUp, Level, Job.Fit, Job.Wit, Job.Grit);
    }
    else
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
    CurrentHP = Job.HPOnLevel[Level];
    CurrentMP = Job.Wit;
    EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, CurrentHP, CurrentMP);
  }
}
