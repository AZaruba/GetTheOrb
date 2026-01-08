using Godot;
using Godot.Collections;

public partial class MonsterTable : Resource
{
  [Export] Array<Monster> Monsters;
  [Export] float[] Weights;

  public Monster GetMonster()
  {
    RandomNumberGenerator rng = new RandomNumberGenerator();
    return Monsters[(int)rng.RandWeighted(Weights)];
  }
}
