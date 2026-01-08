using Godot;
using System;
using System.ComponentModel;

public partial class FightMode : GameMode
{
  private enum PHASE
  {
    PLAYER_TURN,
    MONSTER_TURN,
    DELAYING,
    ANIMATING,
    //ASSESSING,
    FIGHT_OVER
  }

  [Export] Sprite2D EnemySprite;
  [Export] Sprite2D AttackAnimation;
  [Export] Sprite2D EnemyAttackAnimation;
  [Export] AnimationPlayer AnimPlayer;
  [Export] RichTextLabel HPDisplay;
  [Export] RichTextLabel Direction;
  [Export] RichTextLabel MPDisplay;
  [Export] Player Player;

  private Monster CurrentMonster;
  private int MonsterCurrentHP;

  private int PlayerDelay;
  private int MonsterDelay;
  private int DefendModifier;
  private PHASE CurrentPhase;

  private int DelayCap = 10;

  MonsterTable CurrentMonsterTable; // TODO tie tables to floors
  public override void _Ready()
  {
    EventBus.Instance.OnMonsterEncountered += OnMonsterEncountered;
    CurrentMonsterTable = ResourceLoader.Load<MonsterTable>("res://Assets/Data/Monsters/FloorOneMonsterTable.tres");
  }
  public override void ProcessGameMode(double delta)
  {
    if (Player.CurrentHP <= 0)
    {
      EventBus.Instance.EmitSignal(EventBus.SignalName.OnGameOver);
      return;
    }
    if (CurrentPhase == PHASE.PLAYER_TURN)
    {
      RunPlayerTurn();
    }
    else if (CurrentPhase == PHASE.MONSTER_TURN)
    {
      RunMonsterTurn();
    }
    else if (CurrentPhase == PHASE.ANIMATING)
    {
      RunAnimation();
    }
    else if (CurrentPhase == PHASE.DELAYING)
    {
      if (MonsterCurrentHP <= 0)
      {
        CurrentPhase = PHASE.FIGHT_OVER;
        EnemySprite.Visible = false;
        Visible = false;
        Item Drop = CurrentMonster.Treasures.RollForItem();
        if (Drop != null)
        {
          EventBus.Emit(EventBus.SignalName.OnTreasureFound, CurrentMonster.EXP, Drop);
          GD.Print("Say what");
        }
        else
        {
          EventBus.Emit(EventBus.SignalName.OnMonsterDefeated, CurrentMonster.EXP);
        }
        return;
      }

      if (PlayerDelay > DelayCap)
      {
        PlayerDelay %= DelayCap;
        CurrentPhase = PHASE.PLAYER_TURN;
        DefendModifier = 0;
      }
      else if (MonsterDelay > 10)
      {
        MonsterDelay %= DelayCap;
        CurrentPhase = PHASE.MONSTER_TURN;
      }
      else
      {
        PlayerDelay += Player.Job.Fit;
        MonsterDelay += CurrentMonster.Fit;
      }
    }
  }

  private void RunAnimation()
  {
    if (!AnimPlayer.IsPlaying())
    {
      EnemySprite.Visible = true;
      EnemyAttackAnimation.Visible = false;
      AttackAnimation.Visible = false;
      CurrentPhase = PHASE.DELAYING;
    }
  }

  private void RunMonsterTurn()
  {
    // monster logic goes here

    if (StatFunction.CalculateHit(CurrentMonster.Fit, Player.Job.Fit + DefendModifier))
    {
      Player.CurrentHP -= StatFunction.CalculateDamage(CurrentMonster.GetPrimaryStat(), CurrentMonster.Wit, CurrentMonster.Grit, false);
      EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, Player.CurrentHP, Player.CurrentMP);

      EnemyAttackAnimation.Visible = true;
      AnimPlayer.Play("AttackAnims/Enemy");
      CurrentPhase = PHASE.ANIMATING;
    }
    else
    {
      CurrentPhase = PHASE.DELAYING;
    }
  }

  private void RunPlayerTurn()
  {
    int AttackFit = Player.Job.Fit;
    int AttackWit = Player.Job.Wit;
    if (Input.IsActionJustPressed(InputAction.LeftHand))
    {
      // use left hand item
      if (StatFunction.CalculateHit(AttackFit + Player.LeftHandItem.Fit, CurrentMonster.Fit))
      {
        AttackWit += Player.LeftHandItem.Wit;
        MonsterCurrentHP -= StatFunction.CalculateDamage(Player.GetPrimaryStat(true), AttackWit, CurrentMonster.Grit);

        AttackAnimation.Visible = true;
        EnemySprite.Visible = false;
        AnimPlayer.Play("AttackAnims/Slash");
        CurrentPhase = PHASE.ANIMATING;
      }
      else
      {
        CurrentPhase = PHASE.DELAYING;
      }
    }
    if (Input.IsActionJustPressed(InputAction.RightHand))
    {
      // use left hand item
      if (StatFunction.CalculateHit(AttackFit + Player.LeftHandItem.Fit, CurrentMonster.Fit))
      {
        AttackWit += Player.RightHandItem.Wit;
        MonsterCurrentHP -= StatFunction.CalculateDamage(Player.GetPrimaryStat(false), AttackWit, CurrentMonster.Grit);

        AttackAnimation.Visible = true;
        EnemySprite.Visible = false;
        AnimPlayer.Play("AttackAnims/Slash");
        CurrentPhase = PHASE.ANIMATING;
      }
      else
      {
        CurrentPhase = PHASE.DELAYING;
      }
    }
    if (Input.IsActionJustPressed(InputAction.Defend))
    {
      // Defend against attack
      DefendModifier = 3;
      CurrentPhase = PHASE.DELAYING;
    }
    if (Input.IsActionJustPressed(InputAction.Evoke) && Player.CurrentMP > 0)
    {
      // Use Class ability
      Player.CurrentMP--;
      EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, Player.CurrentHP, Player.CurrentMP);
    }
  }

  private void OnMonsterEncountered()
  {
    // TODO how to actually handle monster weights
    CurrentMonster = CurrentMonsterTable.GetMonster();
    MonsterCurrentHP = CurrentMonster.HP;
    Direction.Text = "!";
    PlayerDelay = 0;
    MonsterDelay = 0;
    DefendModifier = 0;
    Visible = true;

    EnemySprite.Texture = CurrentMonster.Sprite;
    EnemySprite.Visible = true;
    CurrentPhase = PHASE.DELAYING;
  }
}
