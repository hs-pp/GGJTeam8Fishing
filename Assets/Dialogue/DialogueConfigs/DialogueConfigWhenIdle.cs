using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DialogueConfigWhenIdle
{
    [SerializeField]
    private List<string> m_dialogueTexts;

    public string GetDialogue()
    {
        return m_dialogueTexts[Random.Range(0, m_dialogueTexts.Count)];
    }
}