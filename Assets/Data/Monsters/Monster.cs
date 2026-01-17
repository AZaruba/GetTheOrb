using Godot;
using System;

public partial class Monster : Resource
{
  [Export] public CompressedTexture2D Sprite;
  [Export] public int Wit;
  [Export] public int Fit;
  [Export] public int Grit;
  [Export] public int HP;
  [Export] public int EXP;

  [Export] public CharacterClass.Stat PrimaryStat;
  [Export] public TreasureTable Treasures;

  [Export] public AttackType ResistantTo = AttackType.NONE;
  [Export] public AttackType WeakTo = AttackType.NONE;

  public int GetPrimaryStat()
  {
    if (PrimaryStat == CharacterClass.Stat.WIT)
    {
      return Wit;
    }
    else if (PrimaryStat == CharacterClass.Stat.FIT)
    {
      return Fit;
    }
    else
    {
      return Grit;
    }
  }
}
