using System;
using System.Collections.Generic;


[Serializable]
public class Player
{
    public string playerID;
    public int days;
    public string username;
    public int score;
    public string petName;
    public int money;
    public bool newGame;

    public Food food;

    public Player(string id, string name)
    {
        playerID = id;
        days = 0;
        food = new Food(0, 0, 0, 0, 0, 0, 0);
        money = 0;
        newGame = true;
        petName = "Hamster";
        score = 0;
        username = name;
    }
}

