using Godot;
using System;

public partial class Item : Resource
{
  [Export] public CompressedTexture2D Sprite;
  [Export] public int Attack;
  [Export] public int Defense;
  [Export] public int Weight;
}
