using Godot;

public partial class LevelUpMode : GameMode
{
  [Export] public RichTextLabel[] Labels;
  [Export] public Timer timer;
  private int CurrentLabel = 0;
  public override void ProcessGameMode(double delta)
  {
    if (timer.IsStopped())
    {
      timer.Start();
    }
    if (CurrentLabel < 3)
    {
      return;
    }
    if (Input.IsActionJustPressed(InputAction.Advance))
    {
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnAdvanceFromLevelUp);
    }
  }

  private void OnTimeInterval()
  {
    if (CurrentLabel == 3)
    {
      timer.Stop();
      return;
    }
    Labels[CurrentLabel].Visible = true;
    CurrentLabel++;
  }

  public void Reset()
  {
    CurrentLabel = 0;
    foreach(RichTextLabel label in Labels)
    {
      label.Visible = false;
    }
  }
}
