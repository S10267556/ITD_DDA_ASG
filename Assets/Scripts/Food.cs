using System;
using UnityEngine;

[Serializable]
public class Food
{
    public int almonds;
    public int broccoli;
    public int caffeine;
    public int carrots;
    public int onions;
    public int strawberries;
    public int sunflowerSeeds;

    public Food(int almonds, int broccoli, int caffeine, int carrots, int onions, int strawberries, int sunflowerSeeds)
    {
        this.almonds = almonds;
        this.broccoli = broccoli;
        this.caffeine = caffeine;
        this.carrots = carrots;
        this.onions = onions;
        this.strawberries = strawberries;
        this.sunflowerSeeds = sunflowerSeeds;
    }

}
