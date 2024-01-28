using System.Collections;
using TMPro;
using UnityEngine;

public class FishRender : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_fishSprite;
    [SerializeField]
    private Transform m_catchPoint;
    [SerializeField]
    private DialogueBubble m_dialogueBubble;
    [SerializeField]
    private TextMeshProUGUI m_nameTagText;

    public FishInstance FishInstance { get; private set; }

    public void Awake()
    {
        m_dialogueBubble.Show(false);
    }

    public void SetFishInstance(FishInstance instance)
    {
        FishInstance = instance;
        m_nameTagText.text = FishInstance.FishName;
    }
    
    public void PlayDialogue(string dialogue)
    {
        if (m_dialogueCoroutine != null)
        {
            StopCoroutine(m_dialogueCoroutine);
        }

        m_dialogueCoroutine = StartCoroutine(TimedDialogue(dialogue));
    }

    public void StopDialogue()
    {
        if (m_dialogueCoroutine != null)
        {
            StopCoroutine(m_dialogueCoroutine);
            m_dialogueBubble.Show(false);
        }
    }

    public Vector3 GetCatchPoint()
    {
        return m_catchPoint.position;
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
