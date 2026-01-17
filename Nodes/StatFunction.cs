using Godot;
using System;

//GD.RandRange(0, EncounterMaxSteps)
public static class StatFunction
{
  public static bool CalculateHit(int AttackFit, int DefendFit)
  {
    float AttackRoll = GD.Randf();

    AttackFit = Math.Max(AttackFit, 1);
    return AttackRoll < (float)AttackFit / (DefendFit + AttackFit);
  }

  public static int CalculateDamage(int AttackPower, int AttackWit, int DefendGrit, AttackType type, AttackType defendWeak = AttackType.NONE, AttackType defendResist = AttackType.NONE , bool Player = true)
  {
    int AttackRoll = GD.RandRange(0,AttackPower+1);
    int damage = Mathf.FloorToInt((float)AttackRoll / ((DefendGrit + 9) / 9));
    if (!Player)
    {
      GD.Print($"Rolled {AttackRoll} but hit for {damage}");
    }
    float CritRoll = GD.Randf();
    if (CritRoll < AttackWit/(AttackWit*2+3))
    {
      GD.Print("Crit!");
      if (Player)
      {
        damage *= 2;
      }
      else
      {
        damage++;
      }
    }

    if (type == defendResist)
    {
      damage = Mathf.FloorToInt((float)damage/2);
    }
    else if (type == defendWeak)
    {
      damage *= 2;
    }

    if (Player && damage == 0)
    {
      damage++;
    }

    if (!Player)
    {
      // cap early damage using some function
      damage = Math.Min(Mathf.CeilToInt((float)(WanderMode.FloorIndex+1)/2),damage);
    }

    GD.Print("Hit For " + damage + " Damage");
    return damage;
  }
}
