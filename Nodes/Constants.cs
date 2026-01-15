using Godot;
using System;
using Godot.Collections;
using System.Reflection.Metadata;

public partial class InputAction
{
  public static readonly string TurnLeft = "TurnLeft";
  public static readonly string TurnRight = "TurnRight";
  public static readonly string MoveForward = "MoveForward";
  public static readonly string TurnAround = "TurnAround";
  public static readonly string LeftHand = "LeftHand";
  public static readonly string RightHand = "RightHand";
  public static readonly string Drop = "Drop";
  public static readonly string Stats = "Stats";
  public static readonly string Advance = "Advance";
  public static readonly string Defend = "Defend";
  public static readonly string Evoke = "Evoke";
  public static readonly string Inspect = "Inspect";
  public static readonly string Location = "Location";
}

public static class Direction
  {
  public enum Facing
  {
    NORTH,
    EAST,
    SOUTH,
    WEST
  }
   public static string[] Chars = ["N","E","S","W"];
}
  public enum AttackType
  {
    ERR_ATK,
    SLASH,
    PIERCE,
    BLUNT,
    MAGIC,
    MISS,
    ENEMY,
    NONE,
  }

public static class Anim
{
  public static readonly Dictionary<AttackType, string> AttackStrings = new()
  {
    { AttackType.SLASH, "AttackAnims/Slash"},
    { AttackType.BLUNT, "AttackAnims/Blunt"},
    { AttackType.PIERCE, "AttackAnims/Pierce"},
    { AttackType.MISS, "AttackAnims/Miss"},
    { AttackType.ENEMY, "AttackAnims/Enemy"},
    { AttackType.NONE, "AttackAnims/Blunt"}
  };
}
