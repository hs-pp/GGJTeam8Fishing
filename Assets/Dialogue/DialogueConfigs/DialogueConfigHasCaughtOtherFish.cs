using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConfigHasCaughtOtherFish : DialogueConfig
{
    [SerializeField]
    private string m_caughtFishUniqueId;

    public bool IsConditionMet()
    {
        return GameStateManager.HasCaughtFish(m_caughtFishUniqueId);
    }
}