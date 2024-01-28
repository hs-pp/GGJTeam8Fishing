using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class DocksDialogue
{
	public List<DialogueItem> Dialogue;
}

[CreateAssetMenu(fileName = "AllDocksDialogue", menuName = "AllDocksDialogue")]

[System.Serializable]
public class DocksDialogueScriptableObject : ScriptableObject
{
	public List<DocksDialogue> AllDocksDialogue;
}

