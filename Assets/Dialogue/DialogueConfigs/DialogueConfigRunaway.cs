using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DialogueConfigRunaway
{
    [SerializeField]
    private List<string> m_dialogueTexts;

    public List<string> GetValidDialogues()
    {
        return m_dialogueTexts;
    }
}
