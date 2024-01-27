using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public int Money;
    public List<string> CaughtFishUniqueIds = new();
}
