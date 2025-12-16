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

    public Food(int almonds, int broccoli, int caffeine, int carrot, int onion, int strawberries, int sunflowerSeeds)
    {
        this.almonds = almonds;
        this.broccoli = broccoli;
        this.caffeine = caffeine;
        this.carrots = carrot;
        this.onions = onion;
        this.strawberries = strawberries;
        this.sunflowerSeeds = sunflowerSeeds;
    }

}
