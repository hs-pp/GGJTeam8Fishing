using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConfigFishWasCaught : DialogueConfig
{
    public List<DialogueItem> Dialogues = new();
}

[Serializable]
public class DialogueItem
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
