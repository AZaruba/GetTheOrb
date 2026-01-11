using Godot;
using System;

public partial class InspectLayer : Node2D
{
  [Export] RichTextLabel AttackLabel;
  [Export] RichTextLabel DefenseLabel;
  [Export] RichTextLabel WeightLabel;

  private const string ATT = "[color=77c7ba]";
  private const string DEF = "[color=b575c7]";
  private const string WEIGHT = "[color=c7a175]";
  private const string BBCLOSE = "[/color]";

  public void Populate(int attack, int defense, int weight)
  {
    AttackLabel.Text = $"{ATT}A" + $"{attack}".PadLeft(2, ' ') + BBCLOSE;
    DefenseLabel.Text = $"{DEF}D" + $"{defense}".PadLeft(2, ' ') + BBCLOSE;
    WeightLabel.Text = $"{WEIGHT}W" + $"{weight}".PadLeft(2, ' ') + BBCLOSE;
  }
}
