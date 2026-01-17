using Godot;
using System;

public partial class SkeletonSpell : MagicSpell
{
  public override void Cast(ref Player player, ref int MonsterHp, ref int MonsterDelay)
  {
    GD.Print("Casting Skel");
    MonsterHp -= player.Job.Wit;
    MonsterDelay = -10;
  }
}
