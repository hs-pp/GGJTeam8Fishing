using System;
using UnityEngine;

[Serializable]
public abstract class DialogueConfig : MonoBehaviour
{
    [SerializeField]
    private string m_dialogueText;
    
    public abstract bool IsConditionMet();
}

public class DialogueConfigHasCaughtFish : DialogueConfig
{
    [SerializeField]
    private string m_fishUniqueId;

    public override bool IsConditionMet()
    {
        return GameStateManager.HasCaughtFish(m_fishUniqueId);
    }
}
