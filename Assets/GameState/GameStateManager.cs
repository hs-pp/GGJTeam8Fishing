using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateManager
{
    private static GameState m_gameState = new GameState();
    
    public static int GetDay()
    {
        return m_gameState.Day;
    }
    public static void IncrementDay()
    {
        m_gameState.Day++;
    }
    
    public static void AddMoney(int amount)
    {
        m_gameState.Money += amount;
    }
    public static void RemoveMoney(int amount)
    {
        m_gameState.Money -= amount;
    }

    public static int GetBaitAmount()
    {
        return m_gameState.BaitAmount;
    }

    public static void DecrementBait()
    {
        m_gameState.BaitAmount--;
    }

    public static void ResetBait()
    {
        m_gameState.BaitAmount = 5;
    }
    
    public static void AddCaughtFish(string uniqueId)
    {
        m_gameState.CaughtFishUniqueIds.Add(uniqueId);
    }
    
    public static bool HasCaughtFish(string uniqueId)
    {
        return m_gameState.CaughtFishUniqueIds.Contains(uniqueId);
    }
    
    public static int NumCaughtFish()
    {
        return m_gameState.CaughtFishUniqueIds.Count;
    }

    public static void ResetGameState()
    {
        m_gameState = new GameState();
    }
}
