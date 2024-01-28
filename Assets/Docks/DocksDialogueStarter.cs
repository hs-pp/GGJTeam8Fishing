using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocksDialogueStarter : MonoBehaviour
{
    [SerializeField] DialogueScene dialogueScene;
    [SerializeField] DocksDialogueScriptableObject docksDialogueLibrary;
	[SerializeField] SceneLoader sceneLoader;

	private void Start()
	{
		dialogueScene.NewScene(docksDialogueLibrary.AllDocksDialogue[GameStateManager.GetDay() - 1].Dialogue);
		dialogueScene.OnDialogueFinish += sceneLoader.LoadFishingScene;
	}
}
