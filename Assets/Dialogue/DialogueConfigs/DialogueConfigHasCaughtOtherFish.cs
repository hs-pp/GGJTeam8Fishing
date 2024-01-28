using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueConfigHasCaughtOtherFish
{
    [SerializeField]
    private List<FishIdToDialogue> m_fishIdToDialogue;

    public List<string> GetValidDialogues()
    {
        List<string> validDialogues = new List<string>();
        foreach (FishIdToDialogue fishIdToDialogue in m_fishIdToDialogue)
        {
            if (fishIdToDialogue.IsConditionMet())
            {
                validDialogues.Add(fishIdToDialogue.Dialogue);
            }
        }

        return validDialogues;
    }
}

[Serializable]
public class FishIdToDialogue
{
    public string CaughtFishId;
    public string Dialogue;
    
    public bool IsConditionMet()
    {
        return GameStateManager.HasCaughtFish(CaughtFishId);
    }
}