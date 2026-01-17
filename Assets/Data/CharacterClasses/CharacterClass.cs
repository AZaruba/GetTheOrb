using Godot;
using System;

public partial class CharacterClass : Resource
{
  public enum Stat
  {
    WIT,
    FIT,
    GRIT
  }
  [Export] public CompressedTexture2D Sprite;
  [Export] public string Name;
  [Export] public Stat PrimaryStat;
  [Export] public MagicSpell Spell;
  [Export] public int BaseWit;
  [Export] public int BaseFit;
  [Export] public int BaseGrit;
  [Export] public int[] WitOnLevel;
  [Export] public int[] FitOnLevel;
  [Export] public int[] GritOnLevel;
  [Export] public int[] HPOnLevel;

  public int Wit;
  public int Fit;
  public int Grit;
}
