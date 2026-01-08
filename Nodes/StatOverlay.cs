using Godot;
using System;

public partial class StatOverlay : Node2D
{
  private const string WIT = "[color=758bc7]";
  private const string FIT = "[color=77c777]";
  private const string GRIT = "[color=c77575]";
  private const string BBCLOSE = "[/color]";

  [Export] RichTextLabel StatDisplay;
  [Export] RichTextLabel LevelDisplay;

  public override void _Ready()
  {
    EventBus.Instance.OnLevelUp += SetStatDisplay;
  }
  
  public void SetStatDisplay(int Level, int Fit, int Wit, int Grit)
  {
    if (Level == 0)
    {
      Level = 1;
    }
    LevelDisplay.Text = $"Lv{Level}";
    StatDisplay.Text = $"{FIT}{Fit}{BBCLOSE}{WIT}{Wit}{BBCLOSE}{GRIT}{Grit}{BBCLOSE}";
  }
}
