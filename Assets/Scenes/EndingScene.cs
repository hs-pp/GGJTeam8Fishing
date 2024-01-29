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
    [SerializeField]
    CanvasGroup _creditsCanvasGroup;

    private bool canQuit;
    private void Start()
    {
        _dialogueScene.NewScene(_dialogueItems);
        _dialogueScene.OnDialogueFinish += FinishScene;
    }

    private void FinishScene()
    {
        StartCoroutine(ShowCredits());
    }

    IEnumerator ShowCredits()
    {
        yield return new WaitForSeconds(2);
        
        while(_creditsCanvasGroup.alpha < 1)
        {
            _creditsCanvasGroup.alpha += Time.deltaTime / 2;
            yield return null;
        }
        
        canQuit = true;
    }

    public void Update()
    {
        if (canQuit && Input.GetKeyDown(KeyCode.Space))
        {
            _sceneLoader.LoadScene("Main Menu");
        }
    }
}
