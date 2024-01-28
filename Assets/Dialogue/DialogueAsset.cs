using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAsset", menuName = "DialogueAsset")]
public class DialogueAsset : ScriptableObject
{
    public List<Dialogue> Dialogues = new();
}

[Serializable]
public class Dialogue
{
    public Speaker Speaker;
    public string DialogueText;
}

public enum Speaker
{
    Teddy,
    Jefferson,
    CaughtFish
}