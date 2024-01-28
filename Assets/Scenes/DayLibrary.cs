using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Day
{
	public List<DialogueItem> DocksDialogue;
	public Sprite NewspaperSprite;
	public AudioClip FishingBGM;
}

[CreateAssetMenu(fileName = "AllDaysLibrary", menuName = "AllDaysLibrary")]

[System.Serializable]
public class DayLibrary : ScriptableObject
{
	public List<Day> Days;
}

