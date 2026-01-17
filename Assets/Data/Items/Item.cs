using Godot;
using System;
using System.Collections.Generic;

public partial class Item : Resource
{

  [Export] public CompressedTexture2D Sprite;
  [Export] public int Attack;
  [Export] public int Defense;
  [Export] public int Weight;
  [Export] public AttackType Type;
  [Export] public ItemID ID;
}
