using Godot;
using System;

public partial class TitleMode : GameMode
{
  [Export] public RichTextLabel[] Labels;
  [Export] public Timer timer;
  private int CurrentLabel = 0;
  public override void ProcessGameMode(double delta)
  {
    if (CurrentLabel < 3)
    {
      return;
    }
    if (Input.IsActionJustPressed(InputAction.Advance))
    {
      SfxPlayer.Instance.PlayConfirmAudio();
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnAdvanceFromTitle);
    }
  }

  private void OnTimeInterval()
  {
    if (CurrentLabel == 3)
    {
      timer.Stop();
      return;
    }
    SfxPlayer.Instance.PlayHitSFX(AttackType.BLUNT);
    Labels[CurrentLabel].Visible = true;
    CurrentLabel++;
  }

  public void Init()
  {
    timer.Start();
  }
}
