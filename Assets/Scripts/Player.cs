using System;
using System.Collections.Generic;

[Serializable]
public class Player
{
    public string playerID;
    public string username;
    public int score;
    public string petName;

    public Dictionary<string, int> foodItems;

    public Player(string id, string name)
    {
        playerID = id;
        username = name;
        score = 0;
        petName = "";
        foodItems = new Dictionary<string, int>();
    }

}
