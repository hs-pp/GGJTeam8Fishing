using System.Collections;
using UnityEditor;
using UnityEngine;

public class FishRender : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_fishSprite;
    [SerializeField]
    private Transform m_catchPoint;
    [SerializeField]
    private float m_visionRadius;
    [SerializeField]
    private DialogueBubble m_dialogueBubble;

    public FishInstance FishInstance { get; private set; }

    public void Awake()
    {
        m_dialogueBubble.Show(false);
    }

    public void SetFishInstance(FishInstance instance)
    {
        FishInstance = instance;
    }
    
    public void Rotate(Vector3 rotate)
    {
        m_fishSprite.transform.rotation = Quaternion.Euler(rotate);
    }
    
    public void PlayDialogue(string dialogue)
    {
        if(m_dialogueCoroutine != null)
            StopCoroutine(m_dialogueCoroutine);
        
        m_dialogueCoroutine = StartCoroutine(TimedDialogue(dialogue));
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

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.forward, m_visionRadius, 5);
    }
}
