using Godot;
using System;

public partial class GameOverMode : GameMode
{
  public override void ProcessGameMode(double delta)
  {
    if (Input.IsActionJustPressed(InputAction.Advance))
    {
      EventBus.Emit(EventBus.SignalName.OnRetry);
    }
  }
}
