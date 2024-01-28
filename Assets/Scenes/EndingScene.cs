using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    [SerializeField]
    private DialogueScene _dialogueScene;
    [SerializeField]
    private List<DialogueItem> _dialogueItems;
    
    private void Start()
    {
        _dialogueScene.NewScene(_dialogueItems);
        _dialogueScene.OnDialogueFinish += FinishScene;
    }

    private void FinishScene()
    {
        //Open main menu
    }
}
