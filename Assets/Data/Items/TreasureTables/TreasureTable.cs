using Godot;
using Godot.Collections;

public partial class TreasureTable : Resource
{
  [Export] Array<Item> Items;
  [Export] float[] Weights;
  public Item RollForItem()
  {
    RandomNumberGenerator rng = new RandomNumberGenerator();
    return Items[(int)rng.RandWeighted(Weights)];
  }
}
