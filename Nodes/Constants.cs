using Godot;
using System;

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
