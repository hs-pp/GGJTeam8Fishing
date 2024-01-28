using System;
using UnityEngine;

[Serializable]
public abstract class DialogueConfig : MonoBehaviour
{
    [SerializeField]
    private string m_dialogueText;
    public string DialogueText => m_dialogueText;
}
