using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public int Day = 4;
    public int Money = 0;
    public int BaitAmount = 5;
    public List<string> CaughtFishUniqueIds = new();
    public int TotalDays = 10;
}
