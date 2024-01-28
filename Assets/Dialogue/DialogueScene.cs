using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class DialogueScene : MonoBehaviour
{
    [SerializeField] DialogueBubble teddyBubble;
    [SerializeField] DialogueBubble fishBubble;
    [SerializeField] DialogueBubble jeffersonBubble;


    private List<DialogueItem> _dialogueItems;
    public Action OnDialogueFinish;

    bool _sceneActive = false;


    // Update is called once per frame
    void Update()
    {
        if(_sceneActive && Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceDialogue();
        }
    }

    //Scene is started here
    public void NewScene(List<DialogueItem> dialogueItems)
    {
        _sceneActive = true;
        _dialogueItems = new List<DialogueItem>(dialogueItems);
        if(_dialogueItems.Count > 0)
            AdvanceDialogue();
    }

    void AdvanceDialogue()
    {
        teddyBubble?.Show(false);
        fishBubble?.Show(false);
        jeffersonBubble?.Show(false);

        if (_dialogueItems.Count > 0)
        {

            switch (_dialogueItems[0].Speaker)
            {
                case Speaker.Teddy:
                    if(teddyBubble != null)
                    {
                        teddyBubble.SetText(_dialogueItems[0].DialogueText);
                        teddyBubble.Show(true);
                    }
                    break;
                case Speaker.CaughtFish:
                    if (fishBubble != null)
                    {
                        fishBubble.SetText(_dialogueItems[0].DialogueText);
                        fishBubble.Show(true);
                    }
                    break;
                case Speaker.Jefferson:
                    if (jeffersonBubble != null)
                    {
                        jeffersonBubble.SetText(_dialogueItems[0].DialogueText);
                        jeffersonBubble.Show(true);
                    }
                    break;
            }
            
            _dialogueItems.RemoveAt(0);
        } else
        {
            _sceneActive = false;
            OnDialogueFinish?.Invoke();
        }
    }

    

}
