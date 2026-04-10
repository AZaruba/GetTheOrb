using Godot;
using System;

public partial class WerewolfSpell : MagicSpell
{
  public override void Cast(ref Player player, ref int MonsterHp, ref int MonsterDelay)
  {
    int BaseDamage = player.Job.Wit + player.Job.Grit;
    if (GD.RandRange(player.Job.Wit, 4) > 3)
    {
      BaseDamage *= 2;
    }

    MonsterHp -= BaseDamage;
  }
}
