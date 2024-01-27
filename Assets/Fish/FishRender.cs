using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FishRender : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_fishSprite;
    [SerializeField]
    private Transform m_catchPoint;
    [SerializeField]
    private DialogueBubble m_dialogueBubble;

    public void Awake()
    {
        m_dialogueBubble.Show(false);
    }
    public void RotateLeft()
    {
        m_fishSprite.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void RotateRight()
    {
        m_fishSprite.transform.rotation = Quaternion.identity;
    }

    public void RotateUp()
    {
        m_fishSprite.transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    
    public void PlayDialogue(string dialogue)
    {
        if(m_dialogueCoroutine != null)
            StopCoroutine(m_dialogueCoroutine);
        
        m_dialogueCoroutine = StartCoroutine(TimedDialogue(dialogue));
    }
    
    private Coroutine m_dialogueCoroutine;

    private IEnumerator TimedDialogue(string dialogue)
    {
        m_dialogueBubble.SetText(dialogue);
        
        m_dialogueBubble.Show(true);
        yield return new WaitForSeconds(4);
        m_dialogueBubble.Show(false);
    }
}
