using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_dayText;

    public void Start()
    {
        m_dayText.text = GameStateManager.GetDay().ToString();
    }
}
