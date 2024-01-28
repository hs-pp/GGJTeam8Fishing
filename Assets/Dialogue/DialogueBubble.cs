using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text;
    
    public void SetText(string text)
    {
        m_text.text = text;
    }
    
    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
