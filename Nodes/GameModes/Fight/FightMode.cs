using System;
using Godot;

public partial class FightMode : GameMode
{
  private enum PHASE
  {
    WAITING_FOR_LOCK,
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

  private bool RepeatAnimation = false;

  private int DelayCap = 10;

  MonsterTable CurrentMonsterTable; // TODO tie tables to floors
  public override void _Ready()
  {
    EventBus.Instance.OnMonsterEncountered += OnMonsterEncountered;
    CurrentMonsterTable = ResourceLoader.Load<MonsterTable>("res://Assets/Data/Monsters/FloorOneMonsterTable.tres");
  }
  public override void _ExitTree()
  {
    EventBus.Instance.OnMonsterEncountered -= OnMonsterEncountered;
  }
  public override void ProcessGameMode(double delta)
  {
    if (CurrentPhase == PHASE.WAITING_FOR_LOCK)
    {
      return;
    }
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
        return;
      }
      else if (MonsterDelay > DelayCap)
      {
        MonsterDelay %= DelayCap;
        CurrentPhase = PHASE.MONSTER_TURN;
        return;
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
      if (RepeatAnimation)
      {
        SfxPlayer.Instance.PlayHitSFX(Player.LeftHandItem.Type);
        RepeatAnimation = false;
        AnimPlayer.Play();
      }
      else
      {
        EnemySprite.Visible = true;
        EnemyAttackAnimation.Visible = false;
        AttackAnimation.Visible = false;
        CurrentPhase = PHASE.DELAYING;
      }
    }
  }

  private void RunMonsterTurn()
  {
    // monster logic goes here

    int PlayerDodge = Player.Job.Fit - Player.LeftHandItem.Weight - Player.RightHandItem.Weight + DefendModifier;
    if (StatFunction.CalculateHit(CurrentMonster.Fit, PlayerDodge))
    {
      int PlayerDefense = Player.Job.Grit + Player.LeftHandItem.Defense + Player.RightHandItem.Defense;
      Player.CurrentHP -= StatFunction.CalculateDamage(CurrentMonster.GetPrimaryStat(), CurrentMonster.Wit, PlayerDefense, AttackType.ENEMY, AttackType.NONE, AttackType.NONE, false);
      EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, Player.CurrentHP, Player.CurrentMP);

      EnemyAttackAnimation.Visible = true;
      AnimPlayer.Play("AttackAnims/Enemy");
        SfxPlayer.Instance.PlayHitSFX(AttackType.ENEMY);
      CurrentPhase = PHASE.ANIMATING;
    }
    else
    {
      EnemyAttackAnimation.Visible = true;
      AnimPlayer.Play("AttackAnims/EnemyMiss");
        SfxPlayer.Instance.PlayHitSFX(AttackType.MISS);
      CurrentPhase = PHASE.ANIMATING;
    }
  }

  private void RunPlayerTurn()
  {
    int AttackFit = Player.Job.Fit;
    int AttackWit = Player.Job.Wit;
    bool DualWield = Player.LeftHandItem.ID == Player.RightHandItem.ID;
    if (Input.IsActionJustPressed(InputAction.LeftHand))
    {
      LeftHandAttack(AttackFit, AttackWit);
      if (DualWield)
      {
        GD.Print("Dual wielding");
        RightHandAttack(AttackFit, AttackWit);
        RepeatAnimation = true;
      }
    }
    if (Input.IsActionJustPressed(InputAction.RightHand))
    {
      RightHandAttack(AttackFit, AttackWit);
      if (DualWield)
      {
        GD.Print("Dual wielding");
        LeftHandAttack(AttackFit, AttackWit);
        RepeatAnimation = true;
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
      Player.Job.Spell.Cast(ref Player, ref MonsterCurrentHP, ref MonsterDelay);

      EventBus.Emit(EventBus.SignalName.OnUpdateHPMP, Player.CurrentHP, Player.CurrentMP);
    }
  }

  private void RightHandAttack(int AttackFit, int AttackWit)
  {
    // use right hand item
    if (StatFunction.CalculateHit(AttackFit - Player.RightHandItem.Weight, CurrentMonster.Fit))
    {
      MonsterCurrentHP -= StatFunction.CalculateDamage(Player.GetPrimaryStat(false) + Player.LeftHandItem.Attack, AttackWit, CurrentMonster.Grit, Player.RightHandItem.Type, CurrentMonster.WeakTo, CurrentMonster.ResistantTo);

      AttackAnimation.Visible = true;
      EnemySprite.Visible = false;
      AnimPlayer.Play(Anim.AttackStrings[Player.RightHandItem.Type]);
      SfxPlayer.Instance.PlayHitSFX(Player.RightHandItem.Type);
      CurrentPhase = PHASE.ANIMATING;
    }
    else
    {
      AttackAnimation.Visible = true;
      EnemySprite.Visible = false;
      AnimPlayer.Play("AttackAnims/Miss");
      SfxPlayer.Instance.PlayHitSFX(AttackType.MISS);
      CurrentPhase = PHASE.ANIMATING;
    }
  }

  private void LeftHandAttack(int AttackFit, int AttackWit)
  {
    // use left hand item
    if (StatFunction.CalculateHit(AttackFit - Player.LeftHandItem.Weight, CurrentMonster.Fit))
    {
      MonsterCurrentHP -= StatFunction.CalculateDamage(Player.GetPrimaryStat(true) + Player.LeftHandItem.Attack, AttackWit, CurrentMonster.Grit, Player.LeftHandItem.Type, CurrentMonster.WeakTo, CurrentMonster.ResistantTo);

      AttackAnimation.Visible = true;
      EnemySprite.Visible = false;
      AnimPlayer.Play(Anim.AttackStrings[Player.LeftHandItem.Type]);
      SfxPlayer.Instance.PlayHitSFX(Player.LeftHandItem.Type);
      CurrentPhase = PHASE.ANIMATING;
    }
    else
    {
      AttackAnimation.Visible = true;
      EnemySprite.Visible = false;
      AnimPlayer.Play("AttackAnims/Miss");
      SfxPlayer.Instance.PlayHitSFX(AttackType.MISS);
      CurrentPhase = PHASE.ANIMATING;
    }
  }

  private void ReleaseAudioLock()
  {
    CurrentPhase = PHASE.DELAYING;
    SfxPlayer.Instance.Finished -= ReleaseAudioLock;
  }

  private void OnMonsterEncountered()
  {
    SfxPlayer.Instance.Finished += ReleaseAudioLock;
    CurrentMonster = CurrentMonsterTable.GetMonster();
    MonsterCurrentHP = CurrentMonster.HP;
    Direction.Text = "!";
    PlayerDelay = GD.RandRange(0, 9);
    MonsterDelay = GD.RandRange(0, 9);
    DefendModifier = 0;
    Visible = true;
    RepeatAnimation = false;

    EnemySprite.Texture = CurrentMonster.Sprite;
    EnemySprite.Visible = true;
    CurrentPhase = PHASE.WAITING_FOR_LOCK;
  }
}
