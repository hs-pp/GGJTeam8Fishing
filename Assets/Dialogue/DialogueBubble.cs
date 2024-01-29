using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text;

    [SerializeField]
    private AudioSource m_audio;
    
    public void SetText(string text)
    {
        m_text.text = text;
    }
    
    public void Show(bool show)
    {
        gameObject.SetActive(show);
        if (m_audio != null)
        {
            m_audio.Play();
        }
    }

    public void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
