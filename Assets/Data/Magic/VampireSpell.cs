using Godot;
using System;

public partial class VampireSpell : MagicSpell
{
  public override void Cast(ref Player player, ref int MonsterHp, ref int MonsterDelay)
  {
    GD.Print("Casting Vamp");
    int Roll = GD.RandRange(player.Job.Wit - 2, player.Job.Wit);
    MonsterHp -= Roll;
    player.CurrentHP = Math.Clamp(player.CurrentHP + Roll, 0, Math.Min(9, player.Job.HPOnLevel[player.Level]+1));
  }
}
