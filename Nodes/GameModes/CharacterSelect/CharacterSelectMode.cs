using Godot;
using System;

public partial class CharacterSelectMode : GameMode
{
  private const string WIT = "[color=758bc7]";
  private const string FIT = "[color=77c777]";
  private const string GRIT = "[color=c77575]";
  private const string BBCLOSE = "[/color]";
  [Export] RichTextLabel ClassName;
  [Export] Sprite2D ClassIcon;
  [Export] RichTextLabel StatDisplay;

  [Export] CharacterClass[] Characters;
  private int CurrentCharacter = 0;
  
  public override void _Ready()
  {
    UpdateDisplay();
  }

  public override void ProcessGameMode(double delta)
  {
    if (Input.IsActionJustPressed(InputAction.TurnLeft))
    {
      CurrentCharacter = (CurrentCharacter + Characters.Length - 1) % Characters.Length;
      UpdateDisplay();
    }
    if (Input.IsActionJustPressed(InputAction.TurnRight))
    {
      CurrentCharacter = (CurrentCharacter + 1) % Characters.Length;
      UpdateDisplay();
    }
    if (Input.IsActionJustPressed(InputAction.Advance))
    {
      SfxPlayer.Instance.PlayConfirmAudio();
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnSelectCharacter, Characters[CurrentCharacter]);
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnLevelUp, 0, Characters[CurrentCharacter].Fit,Characters[CurrentCharacter].Wit,Characters[CurrentCharacter].Grit);
    }
  }

  public void UpdateDisplay()
  {
    ClassIcon.Texture = Characters[CurrentCharacter].Sprite;
    ClassName.Text = Characters[CurrentCharacter].Name;
    StatDisplay.Text = $"{FIT}{Characters[CurrentCharacter].BaseFit}{BBCLOSE}{WIT}{Characters[CurrentCharacter].BaseWit}{BBCLOSE}{GRIT}{Characters[CurrentCharacter].BaseGrit}{BBCLOSE}";
  }
}
