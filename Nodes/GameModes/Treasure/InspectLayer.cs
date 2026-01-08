using Godot;
using System;

public partial class InspectLayer : Node2D
{
  [Export] RichTextLabel FitLabel;
  [Export] RichTextLabel WitLabel;
  [Export] RichTextLabel GritLabel;

  private const string WIT = "[color=758bc7]";
  private const string FIT = "[color=77c777]";
  private const string GRIT = "[color=c77575]";
  private const string BBCLOSE = "[/color]";

  public void Populate(int fit, int wit, int grit)
  {
    WitLabel.Text = $"{WIT}W" + $"{wit}".PadLeft(2, ' ') + BBCLOSE;
    FitLabel.Text = $"{FIT}F" + $"{fit}".PadLeft(2, ' ') + BBCLOSE;
    GritLabel.Text = $"{GRIT}G" + $"{grit}".PadLeft(2, ' ') + BBCLOSE;
  }
}
