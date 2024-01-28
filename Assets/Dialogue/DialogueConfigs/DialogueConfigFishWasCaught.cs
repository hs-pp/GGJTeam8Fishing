using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueConfigFishWasCaught
{
    public List<DialogueItem> Conversation = new();
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
