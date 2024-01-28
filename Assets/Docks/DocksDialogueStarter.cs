using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocksDialogueStarter : MonoBehaviour
{
    [SerializeField] DialogueScene dialogueScene;
    [SerializeField] DayLibrary dayLibrary;
	[SerializeField] SceneLoader sceneLoader;

	private void Start()
	{
		dialogueScene.NewScene(dayLibrary.Days[GameStateManager.GetDay() - 1].DocksDialogue);
		dialogueScene.OnDialogueFinish += sceneLoader.LoadFishingScene;
	}
}
