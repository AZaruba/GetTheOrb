using Godot;
using System;

public partial class Item : Resource
{
  [Export] public CompressedTexture2D Sprite;
  [Export] public int Wit;
  [Export] public int Fit;
  [Export] public int Grit;
}
