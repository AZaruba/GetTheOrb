using Godot;
using System;

public partial class SfxPlayer : AudioStreamPlayer
{
	[Export] AudioStream BluntHitSFX;
	[Export] AudioStream SlashHitSFX;
	[Export] AudioStream PierceHitSFX;
	[Export] AudioStream MissSFX;
	[Export] AudioStream LadderSFX;
	[Export] AudioStream HealingSFX;
	[Export] AudioStream EncounterSFX;
	[Export] AudioStream MoveSFX;
	[Export] AudioStream ConfirmSFX;

	// Called when the node enters the scene tree for the first time.
	public static SfxPlayer Instance;
	public override void _Ready()
	{
		EventBus.Instance.OnMonsterEncountered += PlayEncounterAudio;
		EventBus.Instance.OnHealingTileEncountered += PlayHealingAudio;
		EventBus.Instance.OnLadderEncountered += PlayLadderAudio;
		Instance = this;
	}

    public override void _ExitTree()
    {
		EventBus.Instance.OnMonsterEncountered -= PlayEncounterAudio;
		EventBus.Instance.OnHealingTileEncountered -= PlayHealingAudio;
		EventBus.Instance.OnLadderEncountered -= PlayLadderAudio;
        base._ExitTree();
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PlayEncounterAudio()
	{
		Stop();
		Stream = EncounterSFX;
		Play();
	}
	public void PlayHealingAudio()
	{
		Stream = HealingSFX;
		Play();
	}
	public void PlayLadderAudio()
	{
		Stream = LadderSFX;
		Play();
	}
	public void PlayMoveAudio()
	{
		Stream = MoveSFX;
		Play();
	}
	public void PlayConfirmAudio()
	{
		Stream = ConfirmSFX;
		Play();
	}

	public void PlayHitSFX(AttackType type)
	{
		switch (type)
		{
			case AttackType.SLASH:
		      Stream = SlashHitSFX;
			  break;
			case AttackType.BLUNT:
		      Stream = BluntHitSFX;
			  break;
			case AttackType.PIERCE:
		      Stream = PierceHitSFX;
			  break;
			case AttackType.MISS:
		      Stream = MissSFX;
			  break;
			case AttackType.NONE:
		      Stream = BluntHitSFX;
			  break;
			case AttackType.ENEMY:
		      Stream = SlashHitSFX;
			  break;
		}
		Play();
	}
}
