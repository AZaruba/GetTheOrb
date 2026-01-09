using Godot;
using System;

public partial class DungeonOverlay : Node2D
{
  private const string WIT = "[color=758bc7]";
  private const string FIT = "[color=77c777]";
  private const string GRIT = "[color=c77575]";
  private const string BBCLOSE = "[/color]";

  [Export] RichTextLabel FloorDisplay;
  [Export] RichTextLabel PositionDisplay;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
      EventBus.Instance.OnPlayerMove += OnMove;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnMove(int x, int y, int d)
	{
		PositionDisplay.Text = "X" + $"{x}".PadLeft(2, ' ') + "\nY" + $"{y}".PadLeft(2, ' ');
	}

	public void OnFloorChange(int floor)
	{
		FloorDisplay.Text = "F" + $"{floor+1}".PadLeft(2, ' ');
	}
}
