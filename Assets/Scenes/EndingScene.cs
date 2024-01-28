using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    [SerializeField]
    private DialogueScene _dialogueScene;
    [SerializeField]
    private List<DialogueItem> _dialogueItems;
    [SerializeField]
    SceneLoader _sceneLoader;
    
    private void Start()
    {
        _dialogueScene.NewScene(_dialogueItems);
        _dialogueScene.OnDialogueFinish += FinishScene;
    }

    private void FinishScene()
    {
        //Open main menu
        _sceneLoader.LoadScene("Main Menu"); // Prolly should be a credits scene once we have that
    }
}
